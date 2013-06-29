using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

namespace SatelliteServer
{
    public delegate void CameraCaptureHandler(object sender, Bitmap b);
    class CameraDriver
    {
        public event CameraCaptureHandler CameraCapture;
        public CameraDriver(long hWnd)
	    {
            // init our uc480 object
            m_Hwnd = hWnd;
            m_uc480 = new uc480();

            // enable static messages ( no open camera is needed )		
            m_uc480.EnableMessage(uc480.IS_NEW_DEVICE, (int)hWnd);
            m_uc480.EnableMessage(uc480.IS_DEVICE_REMOVAL, (int)hWnd);        

            // init our image struct and alloc marshall pointers for the uc480 memory
            m_Uc480Images = new UC480IMAGE[IMAGE_COUNT];
            int nLoop = 0;
            for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
            {
                m_Uc480Images[nLoop].pMemory = Marshal.AllocCoTaskMem(4);	// create marshal object pointers
                m_Uc480Images[nLoop].MemID = 0;
                m_Uc480Images[nLoop].nSeqNum = 0;
            }

            m_bDrawing = false;
            m_RenderMode = uc480.IS_RENDER_NORMAL;
	    }

        public void Capture()
        {
            // capture a single image
            if (m_uc480.FreezeVideo(uc480.IS_WAIT) != uc480.IS_SUCCESS)
            {
                throw new Exception("Error freeze image");
            }
        }

        /// <summary>
        /// Connects to the camera
        /// </summary>
        public void Init(long hPictureBox)
        {
            m_PictureBoxHwnd = hPictureBox;

            // if opened before, close now
            if (m_uc480.IsOpen())
            {
                m_uc480.ExitCamera();
            }

            // open a camera
            int nRet = m_uc480.InitCamera(0, (int)hPictureBox);
            if (nRet == uc480.IS_STARTER_FW_UPLOAD_NEEDED)
            {
               throw new Exception("The camera requires starter firmware upload");
            }

            if (nRet != uc480.IS_SUCCESS)
            {
                throw new Exception("Camera init failed");
            }

            uc480.SENSORINFO sensorInfo = new uc480.SENSORINFO();
            m_uc480.GetSensorInfo(ref sensorInfo);

            // Set the image size
            int x = 0;
            int y = 0;
            unsafe
            {
                int nAOISupported = -1;
                IntPtr pnAOISupported = (IntPtr)((uint*)&nAOISupported);
                bool bAOISupported = true;

                // check if an arbitrary AOI is supported
                //if (m_uc480.ImageFormat(uc480.IMGFRMT_CMD_GET_ARBITRARY_AOI_SUPPORTED, pnAOISupported, 4) == uc480.IS_SUCCESS)
                //{
                //    bAOISupported = (nAOISupported != 0);
                //}

                // If an arbitrary AOI is supported -> take maximum sensor size
                if (bAOISupported)
                {
                    x = sensorInfo.nMaxWidth;
                    y = sensorInfo.nMaxHeight;
                }
                // Take the image size of the current image format
                else
                {
                    x = m_uc480.SetImageSize(uc480.IS_GET_IMAGE_SIZE_X, 0);
                    y = m_uc480.SetImageSize(uc480.IS_GET_IMAGE_SIZE_Y, 0);
                }

                m_uc480.SetImageSize(x, y);
            }

            // alloc images
            m_uc480.ClearSequence();
            int nLoop = 0;
            for (nLoop = 0; nLoop < IMAGE_COUNT; nLoop++)
            {
                // alloc memory
                m_uc480.AllocImageMem(x, y, 32, ref m_Uc480Images[nLoop].pMemory, ref m_Uc480Images[nLoop].MemID);
                // add our memory to the sequence
                m_uc480.AddToSequence(m_Uc480Images[nLoop].pMemory, m_Uc480Images[nLoop].MemID);
                // set sequence number
                m_Uc480Images[nLoop].nSeqNum = nLoop + 1;
            }

            m_uc480.SetColorMode(uc480.IS_SET_CM_RGB32);
            m_uc480.EnableMessage(uc480.IS_FRAME, (int)m_Hwnd);

            //btnInit.Enabled = false;
            //btnExit.Enabled = true;
            //btnLive.Enabled = true;
            //btnFreeze.Enabled = true;

            //UpdateInfos();

            //// free image
            //if (DisplayWindow.Image != null)
            //{
            //    DisplayWindow.Image.Dispose();
            //    DisplayWindow.Image = null;
            //}

            // capture a single image
            m_uc480.FreezeVideo(uc480.IS_WAIT);
            //Refresh();
        }

        public void HandleMessage(int message, long lParam, long wParam)
        {
            switch (wParam)
            {
                case uc480.IS_FRAME:
                    if (!m_bDrawing)
                        DrawImage();
                    break;

                case uc480.IS_DEVICE_REMOVAL:
                case uc480.IS_NEW_DEVICE:
                    //UpdateInfos();
                    break;
            }
        }

        private void DrawImage()
        {
            m_bDrawing = true;
            // draw current memory if a camera is opened
            if (m_uc480.IsOpen())
            {
                int num = 0;
                IntPtr pMem = new IntPtr();
                IntPtr pLast = new IntPtr();
                m_uc480.GetActSeqBuf(ref num, ref pMem, ref pLast);
                if (pLast.ToInt32() == 0)
                {
                    m_bDrawing = false;
                    return;
                }

                int nLastID = GetImageID(pLast);
                int nLastNum = GetImageNum(pLast);
                m_uc480.LockSeqBuf(nLastNum, pLast);

                m_pCurMem = pLast;		// remember current buffer for our tootip ctrl

                int width = 0, height = 0, bitspp = 0, pitch = 0, bytespp = 0;
                m_uc480.InquireImageMem(m_pCurMem, GetImageID(m_pCurMem), ref width, ref height, ref bitspp, ref pitch);
                bytespp = (bitspp + 1) / 8;
                
                Bitmap bmp = new Bitmap(m_uc480.GetDisplayWidth(),
                                        m_uc480.GetDisplayHeight(),
                                        pitch,
                                        System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                                        m_pCurMem);
                Bitmap bmp2 = new Bitmap(m_uc480.GetDisplayWidth(), m_uc480.GetDisplayHeight());
                Graphics g = Graphics.FromImage(bmp2);
                g.DrawImage(bmp, new Point(0, 0));
                g.Dispose();

                OnCameraCapture(bmp2);

                //m_uc480.RenderBitmap(nLastID, (int)m_PictureBoxHwnd, m_RenderMode);

                m_uc480.UnlockSeqBuf(nLastNum, pLast);
            }
            m_bDrawing = false;
        }

        int GetImageID( IntPtr pBuffer )
		{
			// get image id for a given memory
			if ( !m_uc480.IsOpen() )
				return 0;

			int i = 0;
			for ( i=0; i<IMAGE_COUNT; i++)
				if ( m_Uc480Images[i].pMemory == pBuffer )
					return m_Uc480Images[i].MemID;
			return 0;
		}
		
		int GetImageNum( IntPtr pBuffer )
		{
			// get number of sequence for a given memory
			if ( !m_uc480.IsOpen() )
				return 0;

			int i = 0;
			for ( i=0; i<IMAGE_COUNT; i++)
				if ( m_Uc480Images[i].pMemory == pBuffer )
					return m_Uc480Images[i].nSeqNum;

			return 0;
		}

        protected virtual void OnCameraCapture(Bitmap b)
        {
            if (CameraCapture != null)
                CameraCapture(this, b);
        }

        private int	m_RenderMode;
        private IntPtr m_pCurMem;
        private uc480 m_uc480;
        private static int IMAGE_COUNT = 4;
        private long m_PictureBoxHwnd;
        private long m_Hwnd;
        bool m_bDrawing;
        private struct UC480IMAGE
        {
            public IntPtr pMemory;
            public int MemID;
            public int nSeqNum;
        }
        private UC480IMAGE[] m_Uc480Images;
    }
}
