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
using System.Net.Sockets;
using Irisys.BlackfinAPI.Interfaces;
using Irisys.BlackfinAPI;

namespace Irisys.Domain
{
    /// <summary>
    /// This is a wrapper around the API
    /// </summary>
    public class BlackfinWrapper
    {



        Blackfin m_blackfinObject = new Blackfin();
        BlackfinEngine m_engine = null;
        Socket m_skt = null;
        bool m_connectionError = false;
        string m_connectionErrorString = "";
        string m_warningString = "";

        /// <summary>
        /// 
        /// </summary>
        public BlackfinWrapper()
        {
            m_warningString = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="skt"></param>
        /// <returns></returns>
        public bool AddToEngine(BlackfinEngine engine, Socket skt)
        {
            m_engine = engine;
            m_skt = skt;

            m_connectionError = false;

            if (m_engine.AddNewCounterEndPoint(m_blackfinObject, skt, ConnectionError))
            {
                return true;
            }
            else
            {
                m_engine.RemoveCounterEndPoint(m_blackfinObject);

                skt.Close();

                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearErrors()
        {
            m_connectionError = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLastWarning()
        {
            return m_warningString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetMACAddress()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetMACAddress())
            {
                return m_blackfinObject.MACAddress;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout - GetMACAddress()");
                    }
                }
            }
        }


        public int GetCountLogPeriod()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetCountLogPeriod())
            {

                return m_blackfinObject.CountLogPeriod;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout -GetCountLogPeriod()");
                    }
                }
            }
        }

        public string GetSiteID()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetSiteID())
            {

                return m_blackfinObject.SiteID;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout -GetSiteID()");
                    }
                }
            }
        }


        public string GetDeviceID()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetDeviceID())
            {

                return m_blackfinObject.DeviceID;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout -GetDeviceID()");
                    }
                }
            }
        }

        public uint GetClientConfigTimeout()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetClientConfigTimeout())
            {

                return m_blackfinObject.ClientConfigTimeout;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout - GetClientConfigTimeout()");
                    }
                }
            }
        }


        public ulong GetUpTime()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetUpTime())
            {

                return m_blackfinObject.UpTime;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout -  GetUpTime()");
                    }
                }
            }
        }


        public string GetLocaleString()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetLocaleString())
            {

                return m_blackfinObject.LocaleString;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout - GetLocaleString()");
                    }
                }
            }
        }

        public IrisysTime.IrisysTimeZone GetUnitTimeZone()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetUnitTimeZone())
            {

                return m_blackfinObject.UnitTimeZone;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout - GetUnitTimeZone()");
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<Count> GetLastNCounts(uint n)
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetLastNCounts(n))
            {
                return m_blackfinObject.Counts;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout - GetLastNCounts(uint n)");
                    }
                }
            }
        }


        public List<Count> GetCounts(DateTime start, DateTime end)
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetCounts(start, end))
            {
                return m_blackfinObject.Counts;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout - GetCounts(DateTime start, DateTime end)");
                    }
                }
            }
        }

        public List<Count> GetCurrentCount()
        {
            m_warningString = string.Empty;

            if (null == m_blackfinObject)
            {
                throw new Exception("object null");
            }

            if (m_connectionError)
            {
                throw new Exception("Outstanding connection error");
            }

            if (m_blackfinObject.GetCurrentCount())
            {
                return m_blackfinObject.Counts;
            }
            else
            {
                if (m_connectionError)
                {
                    throw new Exception(m_connectionErrorString);
                }
                else
                {
                    if (m_warningString != string.Empty)
                    {
                        throw new Exception(m_warningString);
                    }
                    else
                    {
                        //went wrong so throw
                        throw new Exception("Call fail timeout - GetCurrentCount()");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShutdownConnection()
        {
            Socket skt = (Socket)m_engine.RemoveCounterEndPoint(m_blackfinObject);

            if (skt != null)
            {
                skt.Close();
            }
        }



        /// <summary>
        /// Error handler for comms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="errorString"></param>
        private void ConnectionError(Blackfin sender, string errorString, Exception exp, Irisys.BlackfinAPI.BlackfinEngine.ConnectionErrorType type)
        {
            //We will get errors here
            //if we get an error then we want to remove the object from the engine
            //and then close the connection

            switch (type)
            {
                case Irisys.BlackfinAPI.BlackfinEngine.ConnectionErrorType.FATAL:
                    m_connectionError = true;
                    m_connectionErrorString = "*******" + (errorString + this.m_blackfinObject.MACAddress) + "*******";


                    break;
                case Irisys.BlackfinAPI.BlackfinEngine.ConnectionErrorType.WARNING:

                    m_warningString = errorString;
                    break;
            }


        }


    }
}
