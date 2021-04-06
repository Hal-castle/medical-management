using SqlSugar;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Role_Jurisdiction
    {
        /// <summary>
        /// 
        /// </summary>
        public Role_Jurisdiction()
        {
        }

        private System.Int32 _RJ_Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//主键并且自增 （string不能设置自增）
        public System.Int32 RJ_Id { get { return this._RJ_Id; } set { this._RJ_Id = value; } }

        private System.Int32? _Rol_Id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Rol_Id { get { return this._Rol_Id; } set { this._Rol_Id = value; } }

        private System.Int32? _Jur_Id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Jur_Id { get { return this._Jur_Id; } set { this._Jur_Id = value; } }
    }
}