﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace toop_project.src.Logging
{
    public class Logger : ILogger,ISolverLogger,IDisposable
    {
        private enum MessageType
        {
            ERROR,
            DEBUG,
            INFO,
            SOLVER
        };

        public src.GUI.IGUI IGui { private get { return iGui; } set { iGui = value; } }
        private src.GUI.IGUI iGui = null;
        private string defaultLogFile = "../../log/log.txt";
        private System.IO.StreamWriter fileStream;
        private List<string> log = new List<string>();
        private static Logger instance = new Logger();
        private Logger()
        {
            try
            {
                SetFile(defaultLogFile);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Logger open file exception: {0}", e));
            }

        }

        #region ILogger
        private void AddLine(MessageType type, string line)
        {
            string message = String.Format("[{0}][{1}] {2}", type, DateTime.Now, line);
            log.Add(message);
            if (fileStream != null) {
                fileStream.WriteLine(message);
                fileStream.Flush();
            }
            if (iGui != null) {
                iGui.UpdateLog(message);
            }
        }
        public static Logger Instance
        {
            get { return instance; }
        }

        public void SetFile(string filename)
        {
            if (fileStream != null)
                fileStream.Close();
            var dirName = filename.Substring(0, filename.LastIndexOf('/'));
            if (!System.IO.Directory.Exists(dirName))
                System.IO.Directory.CreateDirectory(dirName);
            fileStream = new System.IO.StreamWriter(filename);
            Info("Logger started to work");
        }

        public void Info(string message)
        {
            AddLine(MessageType.INFO, message);
        }

        public void Debug(string message)
        {
            AddLine(MessageType.DEBUG, message);
        }

        public void Error(string message)
        {
            AddLine(MessageType.ERROR, message);
            MessageBox.Show("Error: " + message);
        }
        #endregion

        #region ISolverLogger
        public void AddIterationInfo(int num, double residual)
        {
            AddLine(MessageType.SOLVER, String.Format("Solving: iteration number {0}, residual is {1}", num, residual));
            if (iGui != null) {
                iGui.UpdateIterationLog(num, residual);
            }
        }

        #endregion
        public List<string> Log
        {
            get { return log; }
        }

        #region IDisposable 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool m_Disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (fileStream != null)
                {
                    if (disposing)
                        fileStream.Dispose();
                }
                m_Disposed = true;
            }
        }

        ~Logger()
        {
            Dispose(false);
        }

        #endregion
    }
}