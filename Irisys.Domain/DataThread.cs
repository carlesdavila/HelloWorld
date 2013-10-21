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
using System.Threading;
using System.Net.Sockets;
using System.Net;
using NLog;
using Irisys.BlackfinAPI;
using Irisys.BlackfinAPI.Interfaces;

namespace Irisys.Domain
{
    public class DataThread
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Thread m_thread;
        private BlackfinWrapper m_blackfinWrapper;
        private BlackfinEngine m_engine;
        private Socket m_skt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bf"></param>
        public DataThread(BlackfinEngine engine, Socket skt)
        {

            ThreadStart threadStart = new ThreadStart(ThreadMain);

            m_engine = engine;
            m_skt = skt;

            m_thread = new Thread(threadStart);
            m_thread.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        private void ThreadMain()
        {
            m_blackfinWrapper = new BlackfinWrapper();
            //EstadisticasDetalle lastEstadistica = null;
            int countLogPeriod  = 0;
            int clientTimeout = 0;
            long IdTienda  = 0;
            long IdCliente = 0;
            long IdZona = 0;
            long upTime = 0;
            List<Count> counts = null;
            string clientIPAddress = "";



            if (m_blackfinWrapper.AddToEngine(m_engine, m_skt))
            {
                //Everything went well

                //Get Ip Adress of Client
                clientIPAddress = "" + IPAddress.Parse(((IPEndPoint)m_skt.RemoteEndPoint).Address.ToString());
                
                logger.Info("Camera device connected from ip: {0}", clientIPAddress);
                
            }
            else
            {
                logger.Error("Camera device connection failed");
               
                m_blackfinWrapper.ShutdownConnection();
                return;
            }

            try
            {
                 logger.Info("Gathering Data");

                #region GET DEVICE INFO: MAC ADDRESS , UPTIME, CLIENT TIMEOUT, SITE ID, LOCALE STRING, DEVICE ID, COUNT LOG PERIOD, TIMEZONE

                /**GET MAC ADRESS**/
                //string macAddress = m_blackfinWrapper.GetMACAddress();
                //if (macAddress != null)
                //{
                //    //System.Console.WriteLine(macAddress);
                //}
                //else
                //{
                //    //MAC Address fail
                //    logger.Warn("Failed to get mac address");
                //}


                /**GET UPTIME**/
                ulong ulUpTime = m_blackfinWrapper.GetUpTime();
                if (ulUpTime != 0)
                {
                    //System.Console.WriteLine("Up Time: " + ulUpTime);
                    upTime = Convert.ToInt64(ulUpTime);
                }
                else
                {
                    logger.Warn("Failed to get UpTime");
                }


                /**GET CLIENT TIMEOUT**/
                uint uiClientConfigTimeout = m_blackfinWrapper.GetClientConfigTimeout();
                if (uiClientConfigTimeout != 0)
                {
                    //System.Console.WriteLine("Client Config Timeout: " + uiClientConfigTimeout);
                    clientTimeout = Convert.ToInt32(uiClientConfigTimeout);
                }
                else
                {
                    logger.Warn("Failed to get Client Timeout");
                }


                /**GET SITE ID**/
                string siteID = m_blackfinWrapper.GetSiteID();
                if (siteID != null)
                {
                    //System.Console.WriteLine("Site ID: " + siteID);
                    IdTienda = Convert.ToInt64(siteID.Trim());

                }
                else
                {
                    logger.Warn("Failed to get site ID");
                }

                /**GET LOCALE STRING**/
                string localeString = m_blackfinWrapper.GetLocaleString();
                if (localeString != null)
                {
                    //System.Console.WriteLine("Locale String: " + localeString);
                    IdCliente = Convert.ToInt64(localeString.Trim());

                }
                else
                {
                    logger.Warn("Failed to get localeString");
                }

                /**GET DEVICE ID**/
                string deviceID = m_blackfinWrapper.GetDeviceID();
                if (deviceID != null)
                {
                    //System.Console.WriteLine("Device ID (id Zona): " + deviceID);
                    IdZona = Convert.ToInt32(deviceID.Trim());

                }
                else
                {
                    logger.Warn("Failed to get device ID (id Zona)");
                }


                /**GET COUNT LOG PERIOD**/
                countLogPeriod = m_blackfinWrapper.GetCountLogPeriod();
                if (countLogPeriod != 0)
                {
                    //System.Console.WriteLine("Count Log Period: " + countLogPeriod);
                }
                else
                {
                    logger.Warn("Failed to get Count Log Period");
                }

                /**GET DEVICE TIMEZONE**/
                var unitTimeZone = m_blackfinWrapper.GetUnitTimeZone();
                if (unitTimeZone != null)
                {
                    //System.Console.WriteLine("Unite Time Zone: " + unitTimeZone);
                }
                else
                {
                    logger.Warn("Failed to get Count Log Period");
                }

                logger.Info("UPTIME: {0} CLIENT TIMEOUT: {1} SITE ID: {2} LOCALE STRING: {3} DEVICE ID:{4} COUNT LOG PERIOD: {5} TIMEZONE: {6}",
                            upTime, clientTimeout, siteID, localeString, deviceID, countLogPeriod, unitTimeZone.DisplayName);

                

                #endregion

                Thread.Sleep(100);

                //Recuperamos la ultima estadistica guardada en BBDD
                //var session = NHibernateHelper.GetCurrentSession();
                //using (ITransaction tx = session.BeginTransaction())
                //{
                //    try
                //    {
                //        lastEstadistica = session.Query<EstadisticasDetalle>()
                //                                  .Where(e => e.Id_Cliente == IdCliente)
                //                                  .Where(e => e.Id_Tienda == IdTienda)
                //                                  .OrderByDescending(e => e.Fecha)
                //                                  .FirstOrDefault();

                //    }
                //    catch (Exception ex)
                //    {
                //        tx.Rollback();
                //        logger.ErrorException("Error reecuperar la ultima estadistica guardada en BBDD"+ex.ToString(),ex);
                //    }
                //}

                //Recuperamos las entadisticas de 3 dias. y los guardamos en la lista counts
                //if (lastEstadistica == null)
                //{
                //    logger.Debug("[NHIBERNATE] No hay registros antiguos en la tabla EstadisticasDetalle. Recuperamos 3 dias atras");
                //    //recuperamos  el current count de la camara 

                //    DateTime endDate = m_blackfinWrapper.GetLastNCounts(1).First().countTime;

                //    DateTime startDate = endDate.AddDays(-3);
                    
                //    logger.Debug("[NHIBERNATE] Recuperamos datos desde {0} a {1} ", String.Format("{0:G}", startDate) ,String.Format("{0:G}", endDate));

                //    counts = m_blackfinWrapper.GetCounts(startDate, endDate);

                //}
                //Recuperamos los datos desde inicial y lo guardamos en la lista counts
                //else
                //{
                //    logger.Debug("[NHIBERNATE] Ultimo registro de EstadisticasDetalle: {0} ", String.Format("{0:G}", lastEstadistica.Fecha));

                //    DateTime startDate = lastEstadistica.Fecha;
                //    DateTime endDate = m_blackfinWrapper.GetLastNCounts(1).First().countTime;

                //    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(unitTimeZone.TimeZoneID);
                //    DateTime startDateUTC = TimeZoneInfo.ConvertTimeToUtc(startDate, cstZone);

                //    logger.Debug("[NHIBERNATE] Recuperamos datos desde (UTC) {0} a {1} ", String.Format("{0:G}", startDateUTC) , String.Format("{0:G}", endDate));
                //    counts = m_blackfinWrapper.GetCounts(startDateUTC, endDate);

                //}


                /**INSERT COUNTS TO DATABASE**/
                if (counts != null)
                {
                    logger.Debug("Insert Counts");
                    insertCountsToEstaditicasDetalle(Convert.ToInt64(localeString), Convert.ToInt64(siteID),Convert.ToInt32(IdZona), counts.Distinct(new DistinctCountComparer()).ToList(), unitTimeZone, countLogPeriod);
                }
                else
                {
                    //Fail to get counts
                    logger.Error("Failed to get counts");
                }


                /**UPDATE CAMERA INFO **/
                logger.Debug("Updating camera info");
                updateCameraInf(IdCliente, IdTienda, Convert.ToInt32(IdZona), clientIPAddress, upTime, clientTimeout);

                logger.Debug("Data Gathered");

                Thread.Sleep(100);
            }
            catch (Exception e)
            {
                logger.ErrorException("Error gathering data. ERROR: "+e.ToString(),e);
            }

            //If the engine and the blackfin object are ok then remove the connection
            logger.Info("Closing connection");
            m_blackfinWrapper.ShutdownConnection();
        }

        private void updateCameraInf(long IdCliente, long IdTienda, int IdZona, string clientIPAddress, long upTime, int clientTimeout)
        {
            //var session = NHibernateHelper.GetCurrentSession();
            //using (ITransaction tx = session.BeginTransaction())
            //{ 
            
            //    try{

            //        //Get Camera registry
            //        Camara cameraInf = session.Get<Camara>(new Camara() { Id_Cliente = IdCliente, Id_Tienda = IdTienda, Id_Zona = IdZona});

            //        if (cameraInf != null)
            //        {
            //            cameraInf.LastIP = clientIPAddress;
            //            cameraInf.UpTime = upTime;
            //            cameraInf.LastConnection = DateTime.Now;
            //            cameraInf.ClientTimeout = clientTimeout;

            //            session.Update(cameraInf);
            //        }
            //        else { 
            //            //La camara no esta dada de alta
            //            logger.Error("La camara no esta dada de alta");
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        tx.Rollback();
            //        logger.ErrorException("Error en updateCameraInf(): "+ ex.ToString(), ex);
            //    }

            //    tx.Commit();
            //}
        }

        private void insertCountsToEstaditicasDetalle(long IdCliente, long IdTienda, int idZona, List<Count> counts, IrisysTime.IrisysTimeZone unitTimeZone, int countLogPeriod)
        {
            //Count prevCount = null;
            //Count currCount = null;
            //DateTime dateCount;

            //var session = NHibernateHelper.GetCurrentSession();
            //using (ITransaction tx = session.BeginTransaction())
            //{
            //    try
            //    {   //nos saltamos el primer registro
            //        for (int i = 1; i < counts.Count; i++)
            //        {

            //            prevCount = counts[i - 1];
            //            currCount = counts[i];

            //            dateCount = currCount.countTime;

            //            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(unitTimeZone.TimeZoneID);
            //            DateTime cstCountTime = TimeZoneInfo.ConvertTimeFromUtc(dateCount, cstZone);

            //            var timeSpan = currCount.countTime.Subtract(prevCount.countTime);

            //            //Si tienen el mismo numero de Lines las restamos
            //            if ((currCount.countLines.Count == prevCount.countLines.Count))
            //            {

            //                var numCountLines = currCount.countLines.Zip(prevCount.countLines, (a, b) => Convert.ToInt32(a) - Convert.ToInt32(b) ).ToList();

            //              //  var numCountLines = numCountLinesaux.ConvertAll<int?>(s => Convert.ToInt32(s));

            //                //Si no hay ningun valor negativo en el array de restas y la diferencia es igual al CountlogPeriod , insertamos
            //                if ((!numCountLines.Any(value => value < 0))  && (Convert.ToInt32(timeSpan.TotalSeconds) == countLogPeriod) )
            //                {
                                
            //                    EstadisticasDetalle newEstDetalle = new EstadisticasDetalle();

            //                    newEstDetalle.Id_Cliente = IdCliente;
            //                    newEstDetalle.Id_Tienda = IdTienda;
            //                    newEstDetalle.Id_Zona = idZona;
            //                    newEstDetalle.CountLogPeriod = countLogPeriod;
            //                    newEstDetalle.Fecha = cstCountTime;
            //                    newEstDetalle.Line1 = numCountLines.ElementAtOrDefault(0);
            //                    newEstDetalle.Line2 = numCountLines.ElementAtOrDefault(1);
            //                    newEstDetalle.Line3 = numCountLines.ElementAtOrDefault(2);
            //                    newEstDetalle.Line4 = numCountLines.ElementAtOrDefault(3);
            //                    newEstDetalle.Line5 = numCountLines.ElementAtOrDefault(4);
            //                    newEstDetalle.Line6 = numCountLines.ElementAtOrDefault(5);
            //                    newEstDetalle.Line7 = numCountLines.ElementAtOrDefault(6);
            //                    newEstDetalle.Line8 = numCountLines.ElementAtOrDefault(7);

            //                    logger.Debug("INSERT: {0} {1} {2} {3} Lines: {4} - {5} - {6} - {7} - {8} - {9} - {10} - {11}" ,
            //                                  newEstDetalle.Id_Cliente , newEstDetalle.Id_Tienda, newEstDetalle.Id_Zona , newEstDetalle.Fecha,
            //                                  newEstDetalle.Line1, newEstDetalle.Line2, newEstDetalle.Line3, newEstDetalle.Line4, newEstDetalle.Line5, 
            //                                  newEstDetalle.Line6, newEstDetalle.Line7, newEstDetalle.Line8);

            //                    session.SaveOrUpdate(newEstDetalle);
            //                }
            //                else {  
            //                //Si hay un valor negativo en el array 
            //                    EstadisticasDetalle newEstDetalleErronea = new EstadisticasDetalle();

            //                    newEstDetalleErronea.Id_Cliente = IdCliente;
            //                    newEstDetalleErronea.Id_Tienda = IdTienda;
            //                    newEstDetalleErronea.Id_Zona = idZona;
            //                    newEstDetalleErronea.CountLogPeriod = countLogPeriod;
            //                    newEstDetalleErronea.Fecha = cstCountTime;
            //                    newEstDetalleErronea.MissingData = Convert.ToInt32(timeSpan.TotalSeconds);

            //                    logger.Debug("INSERT MISSING DATA REGISTRY: {0} {1} {2} {3}", newEstDetalleErronea.Id_Cliente ,newEstDetalleErronea.Id_Tienda, newEstDetalleErronea.Id_Zona, newEstDetalleErronea.Fecha);
            //                    session.SaveOrUpdate(newEstDetalleErronea);
                            
            //                }
            //            }
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        tx.Rollback();
                    
            //        logger.ErrorException("Error en insertCountsToEstaditicasDetalle(): "+ex.ToString(), ex);
            //    }

            //    tx.Commit();
            //}
        }

    }

    class DistinctCountComparer : IEqualityComparer<Count>
    {

        public bool Equals(Count x, Count y)
        {
            return DateTime.Compare(x.countTime, y.countTime) == 0;
        }

        public int GetHashCode(Count obj)
        {
            return obj.countTime.GetHashCode();
        }
    }


}
