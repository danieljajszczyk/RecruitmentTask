using System;

namespace RecruitmentTask.Entities
{
    public class Document
    {
        public string Author { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public DateTime AdditionDate { get; set; }

        public Document() { }

        public Document(string author, string fileName, byte[] fileBytes, DateTime additionDate)
        {
            this.Author = author;
            this.FileName = fileName;
            this.FileBytes = fileBytes;
            this.AdditionDate = additionDate;
        }
    }
}
