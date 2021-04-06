using SqlSugar;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Tb_User
    {
        /// <summary>
        /// 
        /// </summary>
        public Tb_User()
        {
        }

        private System.Int32 _Use_Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//主键并且自增 （string不能设置自增）
        public System.Int32 Use_Id { get { return this._Use_Id; } set { this._Use_Id = value; } }

        private System.String _User_photo;
        /// <summary>
        /// 
        /// </summary>
        public System.String User_photo { get { return this._User_photo; } set { this._User_photo = value; } }

        private System.String _User_Account;
        /// <summary>
        /// 
        /// </summary>
        public System.String User_Account { get { return this._User_Account; } set { this._User_Account = value; } }

        private System.String _User_Password;
        /// <summary>
        /// 
        /// </summary>
        public System.String User_Password { get { return this._User_Password; } set { this._User_Password = value; } }

        private System.Int32? _User_sort;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? User_sort { get { return this._User_sort; } set { this._User_sort = value; } }
    }
}