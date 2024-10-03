namespace Tutorial.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; } // Danh sách các cột đã bị ảnh hưởng bởi sự thay đổi
        public string PrimaryKey { get; set; } // Khóa chính của bản ghi bị thay đổi
    }

    #region Ví dụ data mẫu
    /*
    new Audit
    {
        Id = 3,
        UserId = "user456",
        Type = "Delete",
        TableName = "OrderDetails",
        DateTime = DateTime.Now,
        OldValues = "{ \"Quantity\": \"5\", \"Price\": \"19.99\" }",
        NewValues = null,
        AffectedColumns = "Quantity, Price",
        PrimaryKey = "OrderID: 102, ProductID: 55"
    }                                                                   
     */
    #endregion Ví dụ data mẫu
}
