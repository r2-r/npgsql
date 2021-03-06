﻿#region License
// The PostgreSQL License
//
// Copyright (C) 2015 The Npgsql Development Team
//
// Permission to use, copy, modify, and distribute this software and its
// documentation for any purpose, without fee, and without a written
// agreement is hereby granted, provided that the above copyright notice
// and this paragraph and the following two paragraphs appear in all copies.
//
// IN NO EVENT SHALL THE NPGSQL DEVELOPMENT TEAM BE LIABLE TO ANY PARTY
// FOR DIRECT, INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES,
// INCLUDING LOST PROFITS, ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS
// DOCUMENTATION, EVEN IF THE NPGSQL DEVELOPMENT TEAM HAS BEEN ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//
// THE NPGSQL DEVELOPMENT TEAM SPECIFICALLY DISCLAIMS ANY WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE. THE SOFTWARE PROVIDED HEREUNDER IS
// ON AN "AS IS" BASIS, AND THE NPGSQL DEVELOPMENT TEAM HAS NO OBLIGATIONS
// TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using NUnit.Framework;

namespace Npgsql.Tests.Types
{
    /// <summary>
    /// Tests on PostgreSQL numeric types
    /// </summary>
    /// <remarks>
    /// http://www.postgresql.org/docs/current/static/datatype-net-types.html
    /// </remarks>
    class NetworkTypeTests : TestBase
    {
        [Test]
        public void InetV4()
        {
            var expectedIp = IPAddress.Parse("192.168.1.1");
            var expectedInet = new NpgsqlInet(expectedIp, 24);
            var cmd = new NpgsqlCommand("SELECT @p1, @p2, @p3, @p4", Conn);
            var p1 = new NpgsqlParameter("p1", NpgsqlDbType.Inet) { Value = expectedInet };
            var p2 = new NpgsqlParameter { ParameterName = "p2", Value = expectedInet };
            var p3 = new NpgsqlParameter("p3", NpgsqlDbType.Inet) { Value = expectedIp };
            var p4 = new NpgsqlParameter { ParameterName = "p4", Value = expectedIp };
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            var reader = cmd.ExecuteReader();
            reader.Read();

            for (var i = 0; i < 2; i++)
            {
                // Regular type (IPAddress)
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof(IPAddress)));
                Assert.That(reader.GetFieldValue<IPAddress>(i), Is.EqualTo(expectedIp));
                Assert.That(reader[i], Is.EqualTo(expectedIp));
                Assert.That(reader.GetValue(i), Is.EqualTo(expectedIp));
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof(IPAddress)));

                // Provider-specific type (NpgsqlInet)
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof(NpgsqlInet)));
                Assert.That(reader.GetProviderSpecificValue(i), Is.EqualTo(expectedInet));
                Assert.That(reader.GetFieldValue<NpgsqlInet>(i), Is.EqualTo(expectedInet));
                Assert.That(reader.GetString(i), Is.EqualTo(expectedInet.ToString()));
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof(NpgsqlInet)));
            }

            for (var i = 2; i < 4; i++)
            {
                // Regular type (IPAddress)
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof(IPAddress)));
                Assert.That(reader.GetFieldValue<IPAddress>(i), Is.EqualTo(expectedIp));
                Assert.That(reader[i], Is.EqualTo(expectedIp));
                Assert.That(reader.GetValue(i), Is.EqualTo(expectedIp));
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof(IPAddress)));

                // Provider-specific type (NpgsqlInet)
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof(NpgsqlInet)));
                Assert.That(reader.GetProviderSpecificValue(i), Is.EqualTo(new NpgsqlInet(expectedIp)));
                Assert.That(reader.GetFieldValue<NpgsqlInet>(i), Is.EqualTo(new NpgsqlInet(expectedIp)));
                Assert.That(reader.GetString(i), Is.EqualTo(new NpgsqlInet(expectedIp).ToString()));
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof(NpgsqlInet)));
            }

            reader.Dispose();
            cmd.Dispose();
        }

        [Test]
        public void InetV6()
        {
            const string addr = "2001:1db8:85a3:1142:1000:8a2e:1370:7334";
            var expectedIp = IPAddress.Parse(addr);
            var expectedInet = new NpgsqlInet(expectedIp, 24);
            var cmd = new NpgsqlCommand("SELECT @p1, @p2, @p3, @p4", Conn);
            var p1 = new NpgsqlParameter("p1", NpgsqlDbType.Inet) { Value = expectedInet };
            var p2 = new NpgsqlParameter { ParameterName = "p2", Value = expectedInet };
            var p3 = new NpgsqlParameter("p3", NpgsqlDbType.Inet) { Value = expectedIp };
            var p4 = new NpgsqlParameter { ParameterName = "p4", Value = expectedIp };
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            var reader = cmd.ExecuteReader();
            reader.Read();

            for (var i = 0; i < 2; i++)
            {
                // Regular type (IPAddress)
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof (IPAddress)));
                Assert.That(reader.GetFieldValue<IPAddress>(i), Is.EqualTo(expectedIp));
                Assert.That(reader[i], Is.EqualTo(expectedIp));
                Assert.That(reader.GetValue(i), Is.EqualTo(expectedIp));
                Assert.That(reader.GetString(i), Is.EqualTo(addr + "/24"));
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof (IPAddress)));

                // Provider-specific type (NpgsqlInet)
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof (NpgsqlInet)));
                Assert.That(reader.GetProviderSpecificValue(i), Is.EqualTo(expectedInet));
                Assert.That(reader.GetFieldValue<NpgsqlInet>(i), Is.EqualTo(expectedInet));
                Assert.That(reader.GetString(i), Is.EqualTo(expectedInet.ToString()));
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof (NpgsqlInet)));
            }

            for (var i = 2; i < 4; i++)
            {
                // Regular type (IPAddress)
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof(IPAddress)));
                Assert.That(reader.GetFieldValue<IPAddress>(i), Is.EqualTo(expectedIp));
                Assert.That(reader[i], Is.EqualTo(expectedIp));
                Assert.That(reader.GetValue(i), Is.EqualTo(expectedIp));
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof(IPAddress)));

                // Provider-specific type (NpgsqlInet)
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof(NpgsqlInet)));
                Assert.That(reader.GetProviderSpecificValue(i), Is.EqualTo(new NpgsqlInet(expectedIp)));
                Assert.That(reader.GetFieldValue<NpgsqlInet>(i), Is.EqualTo(new NpgsqlInet(expectedIp)));
                Assert.That(reader.GetString(i), Is.EqualTo(new NpgsqlInet(expectedIp).ToString()));
                Assert.That(reader.GetProviderSpecificFieldType(i), Is.EqualTo(typeof(NpgsqlInet)));
            }

            reader.Dispose();
            cmd.Dispose();
        }

        [Test]
        public void Cidr()
        {
            var expectedInet = new NpgsqlInet("192.168.1.0/24");
            var cmd = new NpgsqlCommand("SELECT '192.168.1.0/24'::CIDR", Conn);
            var reader = cmd.ExecuteReader();
            reader.Read();

            // Regular type (IPAddress)
            Assert.That(reader.GetFieldType(0), Is.EqualTo(typeof(NpgsqlInet)));
            Assert.That(reader.GetFieldValue<NpgsqlInet>(0), Is.EqualTo(expectedInet));
            Assert.That(reader[0], Is.EqualTo(expectedInet));
            Assert.That(reader.GetValue(0), Is.EqualTo(expectedInet));
            Assert.That(reader.GetString(0), Is.EqualTo("192.168.1.0/24"));
            Assert.That(reader.GetFieldType(0), Is.EqualTo(typeof(NpgsqlInet)));

            reader.Dispose();
            cmd.Dispose();
        }

        [Test]
        public void Macaddr()
        {
            var expected = PhysicalAddress.Parse("08-00-2B-01-02-03");
            var cmd = new NpgsqlCommand("SELECT @p1, @p2", Conn);
            var p1 = new NpgsqlParameter("p1", NpgsqlDbType.MacAddr) { Value = expected };
            var p2 = new NpgsqlParameter { ParameterName = "p2", Value = expected };
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            var reader = cmd.ExecuteReader();
            reader.Read();

            for (var i = 0; i < cmd.Parameters.Count; i++)
            {
                Assert.That(reader.GetFieldValue<PhysicalAddress>(i), Is.EqualTo(expected));
                Assert.That(reader.GetValue(i), Is.EqualTo(expected));
                Assert.That(reader.GetString(i), Is.EqualTo(expected.ToString()));
                Assert.That(reader.GetFieldType(i), Is.EqualTo(typeof(PhysicalAddress)));
            }

            reader.Dispose();
            cmd.Dispose();
        }

        [Test, IssueLink("https://github.com/npgsql/npgsql/issues/835")]
        public void MacaddrMultiple()
        {
            using (var cmd = new NpgsqlCommand("SELECT unnest(ARRAY['08-00-2B-01-02-03'::MACADDR, '08-00-2B-01-02-04'::MACADDR])", Conn))
            using (var r = cmd.ExecuteReader())
            {
                r.Read();
                var p1 = (PhysicalAddress)r[0];
                r.Read();
                var p2 = (PhysicalAddress)r[0];
                Assert.That(p1, Is.EqualTo(PhysicalAddress.Parse("08-00-2B-01-02-03")));
            }
        }

        // Older tests from here

        [Test]
        public void TestNpgsqlSpecificTypesCLRTypesNpgsqlInet()
        {
            // Please, check http://pgfoundry.org/forum/message.php?msg_id=1005483
            // for a discussion where an NpgsqlInet type isn't shown in a datagrid
            // This test tries to check if the type returned is an IPAddress when using
            // the GetValue() of NpgsqlDataReader and NpgsqlInet when using GetProviderValue();

            var command = new NpgsqlCommand("select '192.168.10.10'::inet;", Conn);
            using (var dr = command.ExecuteReader()) {
                dr.Read();
                var result = dr.GetValue(0);
                Assert.AreEqual(typeof(IPAddress), result.GetType());
            }
        }

        public NetworkTypeTests(string backendVersion) : base(backendVersion) {}
    }
}
