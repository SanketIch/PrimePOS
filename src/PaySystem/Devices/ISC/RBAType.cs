using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice
{
    internal class Card
    {
        #region Card Type
        /// <summary>
        /// Card type allowed for Payment
        /// </summary>
        internal struct cardType
        {
            /// <summary>
            /// EMV smard Card
            /// </summary>
            public string Type02 { get { return "02"; } }
            /// <summary>
            /// Damage or Invalid Card
            /// </summary>
            public string Type99 { get { return "99"; } }
            /// <summary>
            /// WIC smart card
            /// </summary>
            public string Type01 { get { return "01"; } }
        }
        /// <summary>
        /// Card type allowed for Payment
        /// </summary>
        public static cardType CardType;
        #endregion Card Type

        #region Card Status
        internal struct cardstatus
        {
            /// <summary>
            /// Card Inserted
            /// </summary>
            public string Insert { get { return "I"; } }
            /// <summary>
            /// Card Removed
            /// </summary>
            public string Remove { get { return "R"; } }
            /// <summary>
            /// Card has Problem
            /// </summary>
            public string Problem { get { return "P"; } }
        }
        public static cardstatus CardStatus;
        #endregion Card Status
    }
}
