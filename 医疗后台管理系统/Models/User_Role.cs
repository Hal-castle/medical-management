using SqlSugar;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class User_Role
    {
        /// <summary>
        /// 
        /// </summary>
        public User_Role()
        {
        }

        private System.Int32 _UR_Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//主键并且自增 （string不能设置自增）
        public System.Int32 UR_Id { get { return this._UR_Id; } set { this._UR_Id = value; } }

        private System.Int32? _Use_Id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Use_Id { get { return this._Use_Id; } set { this._Use_Id = value; } }

        private System.Int32? _Rol_Id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Rol_Id { get { return this._Rol_Id; } set { this._Rol_Id = value; } }
    }
}