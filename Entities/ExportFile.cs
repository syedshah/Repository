namespace Entities
{
   using System;

   public class ExportFile
   {
     public Guid Id { get; set; }

     public Byte[] FileData { get; set; }

     public string FileName { get; set; }
   }
}
