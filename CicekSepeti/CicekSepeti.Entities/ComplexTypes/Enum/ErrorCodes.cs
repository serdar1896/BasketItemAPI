namespace CicekSepeti.Entities.ComplexTypes.Enum
{
    public class ErrorCodes
    {
        public static ErrorModel UnkownError { get { return new ErrorModel { Code = 500, Text = "Bilinmeyen bir hata olustu!" }; } }
        public static ErrorModel OutOfStock { get { return new ErrorModel { Code = 404, Text = "Urun veya yeterli stok bulunamadi!" }; } }
        public static ErrorModel NoRecordById { get { return new ErrorModel { Code = 404, Text = "Veritanında bu id ye ait kayit bulunamadi!" }; } }
        public static ErrorModel IdFormatIsWrong { get { return new ErrorModel { Code = 400, Text = "Girilen Id 24 hane olmalı!" }; } }
        public static ErrorModel WrongParameter { get { return new ErrorModel { Code = 400, Text = "Yanlis parametre" }; } }

    }

    public class ErrorModel
    {
        public int Code { get; set; }
        public string Text { get; set; }
    }
}
