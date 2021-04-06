using SqlSugar;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Jurisdiction
    {
        /// <summary>
        /// 
        /// </summary>
        public Jurisdiction()
        {
        }

        private System.Int32 _Jur_Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//主键并且自增 （string不能设置自增）
        public System.Int32 Jur_Id { get { return this._Jur_Id; } set { this._Jur_Id = value; } }

        private System.String _Jur_Name;
        /// <summary>
        /// 
        /// </summary>
        public System.String Jur_Name { get { return this._Jur_Name; } set { this._Jur_Name = value; } }

        private System.Int32? _Jur_superior;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Jur_superior { get { return this._Jur_superior; } set { this._Jur_superior = value; } }

        private System.String _Jur_Url;
        /// <summary>
        /// 
        /// </summary>
        public System.String Jur_Url { get { return this._Jur_Url; } set { this._Jur_Url = value; } }

        private System.String _Jur_describe;
        /// <summary>
        /// 
        /// </summary>
        public System.String Jur_describe { get { return this._Jur_describe; } set { this._Jur_describe = value; } }

        private System.String _Jur_icon;
        /// <summary>
        /// 
        /// </summary>
        public System.String Jur_icon { get { return this._Jur_icon; } set { this._Jur_icon = value; } }

        private System.Int32? _Jur_sort;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Jur_sort { get { return this._Jur_sort; } set { this._Jur_sort = value; } }
    }
}