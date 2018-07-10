using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

/*
 * Derivative work based on https://github.com/lduchosal/ipnetwork which was being distributed 
 * under the MIT license when used to seed this portion of the solution.
 * 
 * Adding the 2.x family of this code to the solution via nuget package would be preferred over
 * adding directly in. The current nuget solution requires .Net Core which requires a newer
 * version of Visual Studio.
 */

namespace LukeSkywalker.IPNetwork
{
    public class IPAddressCollection : IEnumerable<IPAddress>, IEnumerator<IPAddress>
    {

        private IPNetwork _ipnetwork;
        private BigInteger _enumerator;

        internal IPAddressCollection(IPNetwork ipnetwork)
        {
            this._ipnetwork = ipnetwork;
            this._enumerator = -1;
        }


        #region Count, Array, Enumerator

        public BigInteger Count
        {
            get
            {
                return this._ipnetwork.Total;
            }
        }

        public IPAddress this[BigInteger i]
        {
            get
            {
                if (i >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("i");
                }
                byte width = this._ipnetwork.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ? (byte)32 : (byte)128;
                IPNetworkCollection ipn = IPNetwork.Subnet(this._ipnetwork, width);
                return ipn[i].Network;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator<IPAddress> IEnumerable<IPAddress>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #region IEnumerator<IpNetwork> Members

        public IPAddress Current
        {
            get { return this[this._enumerator]; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // nothing to dispose
            return;
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            this._enumerator++;
            if (this._enumerator >= this.Count)
            {
                return false;
            }
            return true;

        }

        public void Reset()
        {
            this._enumerator = -1;
        }

        #endregion

        #endregion
    }
}
