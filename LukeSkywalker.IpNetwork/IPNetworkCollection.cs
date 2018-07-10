﻿using System;
using System.Collections;
using System.Collections.Generic;
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
    public class IPNetworkCollection : IEnumerable<IPNetwork>, IEnumerator<IPNetwork>
    {
        private BigInteger _enumerator;
        private byte _cidrSubnet;
        private IPNetwork _ipnetwork;

        private byte _cidr
        {
            get { return this._ipnetwork.Cidr; }
        }
        private BigInteger _broadcast
        {
            get { return IPNetwork.ToBigInteger(this._ipnetwork.Broadcast); }
        }
        private BigInteger _lastUsable
        {
            get { return IPNetwork.ToBigInteger(this._ipnetwork.LastUsable); }
        }
        private BigInteger _network
        {
            get { return IPNetwork.ToBigInteger(this._ipnetwork.Network); }
        }

        internal IPNetworkCollection(IPNetwork ipnetwork, byte cidrSubnet)
        {

            int maxCidr = ipnetwork.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ? 32 : 128;
            if (cidrSubnet > maxCidr)
            {
                throw new ArgumentOutOfRangeException("cidrSubnet");
            }

            if (cidrSubnet < ipnetwork.Cidr)
            {
                throw new ArgumentException("cidr");
            }

            this._cidrSubnet = cidrSubnet;
            this._ipnetwork = ipnetwork;
            this._enumerator = -1;
        }

        #region Count, Array, Enumerator

        public BigInteger Count
        {
            get
            {
                BigInteger count = BigInteger.Pow(2, this._cidrSubnet - this._cidr);
                return count;
            }
        }

        public IPNetwork this[BigInteger i]
        {
            get
            {
                if (i >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("i");
                }

                BigInteger last = this._ipnetwork.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6
                    ? this._lastUsable : this._broadcast;
                BigInteger increment = (last - this._network) / this.Count;
                BigInteger uintNetwork = this._network + ((increment + 1) * i);
                IPNetwork ipn = new IPNetwork(uintNetwork, this._ipnetwork.AddressFamily, this._cidrSubnet);
                return ipn;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator<IPNetwork> IEnumerable<IPNetwork>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #region IEnumerator<IpNetwork> Members

        public IPNetwork Current
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
