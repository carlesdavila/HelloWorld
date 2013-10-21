/*!
 * Blackfin API (.NET)
 * Server Sample 3
 * 
 * This sample application listens for incoming client connections from Irisys Blackfin
 * devices. For each connection received by the server a new DataThread is created to
 * process it. The thread creates a wrapper around the Blackfin object which handles
 * connecting it to the engine and deals with any comms failures. The data thread
 * instructs the wrapper to execute a variety of API functions and, once completed,
 * closes the connection. Please refer to the API documentation for more information.
 * 
 * This software is provided 'as-is', without any express or implied warranty. In no
 * event will the authors be held liable for any damages arising from the use of this
 * software.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using IrisysDevices.BlackfinCore;
using Irisys.BlackfinAPI;
using NLog;

namespace Irisys.Domain
{
    public class BlackfinServer
    {


        private static Logger logger = LogManager.GetCurrentClassLogger();

        BlackfinEngine m_engine = new BlackfinEngine();
        TcpListener m_serverSocket = null;

        /// <summary>
        /// Gets the first IPv4 address for the local machine
        /// </summary>
        /// <returns></returns>
        public IPAddress GetServerAddress()
        {
            IPHostEntry host = Dns.GetHostEntry("");
            foreach (IPAddress addr in host.AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                    return addr;
            }

            return IPAddress.Any;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StartServer(int portNumber)
        {
            try
            {
                m_serverSocket = new TcpListener(GetServerAddress(), portNumber);

                if (m_serverSocket == null)
                {
                    return false;
                }

                m_serverSocket.Start();

                logger.Info("Starting server at Port: {0}", portNumber);

                m_serverSocket.BeginAcceptSocket(new AsyncCallback(ServerConnectCallback), m_serverSocket);

                m_engine.StartEngine();
            }
            catch (Exception e)
            {
                //System.Console.WriteLine(e.Message);

                logger.FatalException("Cannot start server Error:", e);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShutdownServer()
        {
            m_serverSocket.Stop();

            m_engine.ShutdownEngine();

            logger.Info("Server shutdown");

        }


        /// <summary>
        /// This method is called when a client connects in to the server socket
        /// </summary>
        /// <param name="ar"></param>
        private void ServerConnectCallback(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;

            try
            {
                Socket clientSocket = listener.EndAcceptSocket(ar);

                DataThread newDataThread = new DataThread(m_engine, clientSocket);

                //Once we've accepted setup for another connection
                m_serverSocket.BeginAcceptSocket(new AsyncCallback(ServerConnectCallback), m_serverSocket);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

        }

    }
}
