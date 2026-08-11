using System;
using System.ComponentModel.DataAnnotations;

namespace GHM.Infrastructure.Models
{
    public class EntityBase<T>
    {
        /// <summary>
        /// Mã đối tượng.
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Thời gian tạo.
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreatorId { get; set; }

        /// <summary>
        /// Tên người tạo.
        /// </summary>
        public string CreatorFullName { get; set; }

        /// <summary>
        /// Mã kiêm tra khi update.
        /// </summary>
        [MaxLength(50)]
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Lần chỉnh sửa cuối cùng.
        /// </summary>
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Mã người cập nhật cuối cùng.
        /// </summary>
        public string LastUpdateUserId { get; set; }

        /// <summary>
        /// Tên người cập nhật cuối cùng.
        /// </summary>
        public string LastUpdateFullName { get; set; }

        public EntityBase()
        {
            LastUpdate = null;
            CreateTime = DateTime.Now;
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }
    }
}
