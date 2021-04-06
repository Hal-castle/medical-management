using SqlSugar;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Tb_Role
    {
        /// <summary>
        /// 
        /// </summary>
        public Tb_Role()
        {
        }

        private System.Int32 _Role_Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//主键并且自增 （string不能设置自增）
        public System.Int32 Role_Id { get { return this._Role_Id; } set { this._Role_Id = value; } }

        private System.String _Role_Name;
        /// <summary>
        /// 
        /// </summary>
        public System.String Role_Name { get { return this._Role_Name; } set { this._Role_Name = value; } }

        private System.String _Role_Describe;
        /// <summary>
        /// 
        /// </summary>
        public System.String Role_Describe { get { return this._Role_Describe; } set { this._Role_Describe = value; } }

        private System.Int32? _Role_sort;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Role_sort { get { return this._Role_sort; } set { this._Role_sort = value; } }
    }
}