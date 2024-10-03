using Microsoft.EntityFrameworkCore;
using Tutorial.Commons.Enums;
using Tutorial.Commons.Utils;

namespace Tutorial.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ProductDbContext()
        {
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        private void BeforeSaveChanges()
        {
            /*
             * - ChangeTracker: giúp theo dõi các trạng thái của từng thực thể trong DbContext
             * 
             * - DetectChanges(): phương thức đảm bảo tất cả các thay đổi (thêm/sửa/xóa) trên 
             * các đối tượng được theo dõi bởi DbContext đã được cập nhật trước khi lưu vào DB
             */
            ChangeTracker.DetectChanges();

            // Tạo danh sách lưu trữ các bản ghi dựa trên thay đổi được phát hiện
            var auditEntries = new List<AuditEntry>();

            // Duyệt qua các thay đổi thông qua phương thức Entries()
            foreach (var entry in ChangeTracker.Entries())
            {
                // Bỏ qua các thực thể không liên kết với DbContext (Detached) hoặc không có thay đổi (Unchanged)
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                // Tạo đối audit để lưu log
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntries.Add(auditEntry);

                // Duyệt qua từng thuộc tính của thực thể thông qua phương thức Properties
                foreach (var property in entry.Properties)
                {
                    // Lấy tên thực thể
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    // entry.State: lấy trang thái của thực thể (thêm/sửa/xóa)
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            auditEntry.UserId = entry.Property("CreatedBy").CurrentValue != null ? entry.Property("CreatedBy").CurrentValue.ToString() : "Null";
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.UserId = entry.Property("LastModifiedBy").CurrentValue != null ? entry.Property("LastModifiedBy").CurrentValue.ToString() : "Null";
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                                auditEntry.UserId = entry.Property("LastModifiedBy").CurrentValue != null ? entry.Property("LastModifiedBy").CurrentValue.ToString() : "Null";
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }


        #region DBSet
        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Product> Products { get; set; }
        #endregion DBSet
    }
}
