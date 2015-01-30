 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Nexdox.Composer;


namespace nugen99
{
  using Entities;

  using ServiceInterfaces;

    #region ZipPackageCollection
    public class ZipPackageCollection : System.Collections.CollectionBase
    {
        public Guid DocumentSetID = Guid.NewGuid();

        public void Add(ZipPackage aZipPackage)
        {
            List.Add(aZipPackage);
        }

        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
            {
                //Exception here...
            }
            else
            {
                List.RemoveAt(index);
            }
        }

        public ZipPackage Item(int Index)
        {
            // The appropriate item is retrieved from the List object and
            // explicitly cast to the Widget type, then returned to the 
            // caller.
            return (ZipPackage)List[Index];
        }

        public void SaveToDatabase(IXmlFileService xmlFileService, 
          IZipFileService zipFileService, IConFileService conFileService, IDocTypeService docTypeService, IManCoService manCoService, ApplicationInfo appInfo)
        {
            try
            {
                NexdoxMessaging.SendMessage("    Adding Data to SQL Database", true, this);

                var bigZip = (from ZipPackage z in Statics.zipPackage.InnerList
                              where z.IsBigZip
                              select z).FirstOrDefault();

                foreach (ZipPackage zp in Statics.zipPackage)
                {
                    switch (Path.GetExtension(zp.FileName).ToLower())
                    {
                        case ".xml":

                        DocType docType = docTypeService.GetDocType(zp.DocumentType);
                        if (docType == null)
                        {
                          throw new Exception(string.Format("Document type {0} not found in unity database", zp.DocumentType));
                        }

                         ManCo manCo = manCoService.GetManCo(zp.ManCoID);
                         if (manCo == null)
                         {
                           throw new Exception(string.Format("Man Co {0} not found in unity database", zp.ManCoID));
                         }

                            xmlFileService.CreateXmlFile(
                                        Statics.zipPackage.DocumentSetID.ToString(),
                                        zp.FileName,
                                        zp.ParentZipFileName,
                                        zp.Offshore,
                                        docType.Id,
                                        manCo.Id,
                                        0,
                                        string.Empty,
                                        bigZip.FileName,
                                        DateTime.Now,
                                        appInfo.NexdoxGlobalRunID.ToString(),
                                        File.GetLastWriteTime(appInfo.InputPath));
                            break;
                        case ".zip":
                            if (zp.IsBigZip)
                            {
                                zipFileService.CreateBigZipFile(Statics.zipPackage.DocumentSetID.ToString(), zp.FileName, File.GetLastWriteTime(appInfo.InputPath));    
                            }
                            else
                            {
                                zipFileService.CreateLittleZipFile(Statics.zipPackage.DocumentSetID.ToString(), zp.FileName, zp.ParentZipFileName, File.GetLastWriteTime(appInfo.InputPath));  
                            }
                            break;
                        case ".con":
                            conFileService.CreateConFile(Statics.zipPackage.DocumentSetID.ToString(), zp.FileName, zp.ParentZipFileName, File.GetLastWriteTime(appInfo.InputPath));
                            break;
                    }

                    /*SqlCommand sqlComm = new SqlCommand("sp_InsertInputFile", sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlParameter InputFileName = sqlComm.Parameters.Add("@INPUTFILENAME", SqlDbType.VarChar);
                    InputFileName.Direction = ParameterDirection.Input;
                    InputFileName.Value = zp.FileName;

                    SqlParameter DocType = sqlComm.Parameters.Add("@DOCTYPE", SqlDbType.VarChar);
                    DocType.Direction = ParameterDirection.Input;
                    DocType.Value = zp.DocumentType;

                    SqlParameter ParentDocFileName = sqlComm.Parameters.Add("@PARENTDOCFILENAME", SqlDbType.VarChar);
                    ParentDocFileName.Direction = ParameterDirection.Input;
                    ParentDocFileName.Value = zp.ParentZipFileName;

                    SqlParameter BigZip = sqlComm.Parameters.Add("@BIGZIP", SqlDbType.Bit);
                    BigZip.Direction = ParameterDirection.Input;
                    BigZip.Value = zp.IsBigZip;

                    SqlParameter LittleZip = sqlComm.Parameters.Add("@LITTLEZIP", SqlDbType.Bit);
                    LittleZip.Direction = ParameterDirection.Input;
                    LittleZip.Value = zp.IsLittleZip;

                    SqlParameter DocumentSetID = sqlComm.Parameters.Add("@DOCUMENTSET_ID", SqlDbType.UniqueIdentifier);
                    DocumentSetID.Direction = ParameterDirection.Input;
                    DocumentSetID.Value = Statics.zipPackage.DocumentSetID;

                    SqlParameter OffShore = sqlComm.Parameters.Add("@OFFSHORE", SqlDbType.Bit);
                    OffShore.Direction = ParameterDirection.Input;
                    OffShore.Value = zp.Offshore;

                    SqlParameter ManCo = sqlComm.Parameters.Add("@MANCO", SqlDbType.VarChar);
                    ManCo.Direction = ParameterDirection.Input;
                    ManCo.Value = zp.ManCoID.ToString();

                    SqlParameter Domicile = sqlComm.Parameters.Add("@DOMICILE", SqlDbType.VarChar);
                    Domicile.Direction = ParameterDirection.Input;
                    Domicile.Value = zp.Domicile;

                    SqlParameter StatusID = sqlComm.Parameters.Add("@STATUS_ID", SqlDbType.Int);
                    StatusID.Direction = ParameterDirection.Input;
                    StatusID.Value = zp.StatusID;

                    SqlParameter InputDateCreation = sqlComm.Parameters.Add("@INPUTCREATIONDATE", SqlDbType.DateTime);
                    InputDateCreation.Direction = ParameterDirection.Input;
                    InputDateCreation.Value = zp.InputCreationDate;

                    SqlDataReader myReader = sqlComm.ExecuteReader();
                    myReader.Close();*/
                }

            }
            catch (Exception e)
            {
                throw NexdoxMessaging.Exception(e.Message, this);
            }
        }

        public void SaveToOutputDir(string OutputDir,string GRID)
        {
            if (Statics.zipPackage.Count != 0)
            {
                using (StreamWriter unityImport = new StreamWriter(File.Create(OutputDir + "[" + GRID + "]" + "-UnityImport.csv")))
                {
                    unityImport.WriteLine("\"FileName\",\"ParentFileName\",\"DocType\",\"BigZip\",\"LittleZip\",\"DocumentSetID\",\"Onshore\",\"ManCo\",\"DomicileID\",\"StatusID\",\"InputDateCreation\"");
                    foreach (ZipPackage zp in Statics.zipPackage)
                    {
                        try
                        {
                            unityImport.WriteLine("\"" + zp.FileName + "\",\""
                                                        + zp.ParentZipFileName + "\",\""
                                                        + zp.DocumentType + "\",\""
                                                        + zp.IsBigZip.ToString() + "\",\""
                                                        + zp.IsLittleZip.ToString() + "\",\""
                                                        + Statics.zipPackage.DocumentSetID.ToString() + "\",\""
                                                        + zp.Offshore.ToString() + "\",\""
                                                        + (string.IsNullOrEmpty(zp.ManCoID) ? "0" : zp.ManCoID) + "\",\""
                                                        + zp.Domicile.ToString() + "\",\""
                                                        + zp.StatusID.ToString() + "\",\""
                                                        + zp.InputCreationDate.ToString() + "\""
                                                        );
                        }
                        catch (Exception)
                        {
                            
                            throw;
                        }
                            
                    }
                }
            }
        }

    }
    #endregion

    #region ZipPackage
    public class ZipPackage 
    {
        public string FileName { get; set; }
        public string DocumentType { get; set; }
        public string ParentZipFileName { get; set; }
        public bool IsBigZip { get; set; }
        public bool IsLittleZip { get; set; }
        public bool Offshore { get; set; }
        public string ManCoID { get; set; }
        public string Domicile { get; set; }
        public int StatusID { get; set; }
        public DateTime InputCreationDate { get; set; }

        public ZipPackage()
        {
            Domicile = string.Empty;
        }
    }
    #endregion  
}
