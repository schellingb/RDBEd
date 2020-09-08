/*  RDBEd - Retro RDB & DAT Editor
 *  Copyright (C) 2020 - Bernhard Schelling
 *
 *  RDBEd is free software: you can redistribute it and/or modify it under the terms
 *  of the GNU General Public License as published by the Free Software Found-
 *  ation, either version 3 of the License, or (at your option) any later version.
 *
 *  RDBEd is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 *  PURPOSE.  See the GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along with RDBEd.
 *  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

[assembly: System.Reflection.AssemblyProduct("RDBEd")]
[assembly: System.Reflection.AssemblyTitle("RDBEd - Retro RDB & DAT Editor")]
[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.0.0")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
namespace RDBEd { static class About { public const string Text = "RDBEd - Retro RDB & DAT Editor\n\nhttps://github.com/schellingb/RDBEd"; } }

namespace RDBEd
{
    public enum EProgressState : byte { Ready, Counting, Querying, Applying }
    public enum EProgressType : byte { CountInit, CountIncrement, Finish };
    public enum EFieldFlag : byte { None = 0, Modified = 0x1, Warning = 0x2 };
    public enum ERowFlag : byte { None = 0, Modified = 0x1, FromFile = 0x2, WarningsMask = 0xF0, WarningNotUnique = 0x10, WarningToolUnifyError = 0x20, WarningToolMergeError = 0x40, WarningInvalid = 0x80 };
    public enum EFieldIndices : int { Name, Description, Genre, RomName, Size, Users, Release, Rumble, Analog, Coop, EnhancementHW, Franchise,
        OriginalTitle, Developer, Publisher, Origin,Region,Tags, CRC, MD5, SHA1, Serial, COUNT };

    class Entry
    {
        public static Color[] CellFieldColors = new Color[]
        {
            SystemColors.Window, // Default
            Color.Orange,        // Modified
            Color.Red,           // Warning
            Color.Red,           // Modified | Warning
        };

        public string   Name,          OrgName;           public string   ColName          { get { return Name;           } set { Set(EFieldIndices.Name,          ref Name,          OrgName,          value); } }
        public string   Description,   OrgDescription;    public string   ColDescription   { get { return Description;    } set { Set(EFieldIndices.Description,   ref Description,   OrgDescription,   value); } }
        public string   Genre,         OrgGenre;          public string   ColGenre         { get { return Genre;          } set { Set(EFieldIndices.Genre,         ref Genre,         OrgGenre,         value); } }
        public string   RomName,       OrgRomName;        public string   ColRomName       { get { return RomName;        } set { Set(EFieldIndices.RomName,       ref RomName,       OrgRomName,       value); } }
        public uint     Size,          OrgSize;           public uint     ColSize          { get { return Size;           } set { Set(EFieldIndices.Size,          ref Size,          OrgSize,          value); } }
        public uint     Users,         OrgUsers;          public uint     ColUsers         { get { return Users;          } set { Set(EFieldIndices.Users,         ref Users,         OrgUsers,         value); } }
        public DateTime Release,       OrgRelease;        public DateTime ColRelease       { get { return Release;        } set { Set(EFieldIndices.Release,       ref Release,       OrgRelease,       value); } }
        public bool     Rumble,        OrgRumble;         public bool     ColRumble        { get { return Rumble;         } set { Set(EFieldIndices.Rumble,        ref Rumble,        OrgRumble,        value); } }
        public bool     Analog,        OrgAnalog;         public bool     ColAnalog        { get { return Analog;         } set { Set(EFieldIndices.Analog,        ref Analog,        OrgAnalog,        value); } }
        public bool     Coop,          OrgCoop;           public bool     ColCoop          { get { return Coop;           } set { Set(EFieldIndices.Coop,          ref Coop,          OrgCoop,          value); } }
        public string   EnhancementHW, OrgEnhancementHW;  public string   ColEnhancementHW { get { return EnhancementHW;  } set { Set(EFieldIndices.EnhancementHW, ref EnhancementHW, OrgEnhancementHW, value); } }
        public string   Franchise,     OrgFranchise;      public string   ColFranchise     { get { return Franchise;      } set { Set(EFieldIndices.Franchise,     ref Franchise,     OrgFranchise,     value); } }
        public string   OriginalTitle, OrgOriginalTitle;  public string   ColOriginalTitle { get { return OriginalTitle;  } set { Set(EFieldIndices.OriginalTitle, ref OriginalTitle, OrgOriginalTitle, value); } }
        public string   Developer,     OrgDeveloper;      public string   ColDeveloper     { get { return Developer;      } set { Set(EFieldIndices.Developer,     ref Developer,     OrgDeveloper,     value); } }
        public string   Publisher,     OrgPublisher;      public string   ColPublisher     { get { return Publisher;      } set { Set(EFieldIndices.Publisher,     ref Publisher,     OrgPublisher,     value); } }
        public string   Origin,        OrgOrigin;         public string   ColOrigin        { get { return Origin;         } set { Set(EFieldIndices.Origin,        ref Origin,        OrgOrigin,        value); } }
        public string   Region,        OrgRegion;         public string   ColRegion        { get { return Region;         } set { Set(EFieldIndices.Region,        ref Region,        OrgRegion,        value); } }
        public string   Tags,          OrgTags;           public string   ColTags          { get { return Tags;           } set { Set(EFieldIndices.Tags,          ref Tags,          OrgTags,          value); } }
        public string   CRC,           OrgCRC;            public string   ColCRC           { get { return CRC;            } set { Set(EFieldIndices.CRC,           ref CRC,        OrgCRC,  FixHex(value,  8)); } } //hex (4 byte)
        public string   MD5,           OrgMD5;            public string   ColMD5           { get { return MD5;            } set { Set(EFieldIndices.MD5,           ref MD5,        OrgMD5,  FixHex(value, 32)); } } //hex (16 byte)
        public string   SHA1,          OrgSHA1;           public string   ColSHA1          { get { return SHA1;           } set { Set(EFieldIndices.SHA1,          ref SHA1,       OrgSHA1, FixHex(value, 40)); } } //hex (20 byte)
        public string   Serial,        OrgSerial;         public string   ColSerial        { get { return Serial;         } set { Set(EFieldIndices.Serial,        ref Serial,        OrgSerial,        value); } } //binary/ascii

        // Other fields that exist in some data sources, currently not loaded/processed/saved in this program
        //uint   FamitsuRating;
        //uint   EdgeRating;
        //uint   EdgeIssue;
        //string EdgeReview;
        //string Barcode;
        //string EsrbRating;
        //string ElspaRating;
        //string PegiRating;
        //string CeroRating;
        //uint   TgdbRating;  

        public EFieldFlag[] FieldFlags = new EFieldFlag[22];
        public ERowFlag RowFlags;
        string SearchCache = null;

        public void FlagModified(EFieldIndices idx, bool f)
        {
            if (((FieldFlags[(int)idx] & EFieldFlag.Modified) != 0) == f) return;
            FieldFlags[(int)idx] = (f ? (FieldFlags[(int)idx] | EFieldFlag.Modified) : (FieldFlags[(int)idx] & ~EFieldFlag.Modified));
            if (f) RowFlags |= ERowFlag.Modified;
            else
            {
                bool hasModification = false;
                foreach (EFieldFlag ff in FieldFlags) { if ((ff & EFieldFlag.Modified) != 0) { hasModification = true; break; } }
                if (!hasModification) RowFlags &= ~ERowFlag.Modified;
            }
            Data.CountModification(f ? 1 : -1);
        }

        public void FlagWarning(EFieldIndices idx, bool f, ERowFlag rowFlag = ERowFlag.WarningsMask)
        {
            EFieldFlag mod = (rowFlag == ERowFlag.Modified ? EFieldFlag.Modified : EFieldFlag.Warning);
            if (((FieldFlags[(int)idx] & mod) != 0) == f) return;
            FieldFlags[(int)idx] = (f ? (FieldFlags[(int)idx] | mod) : (FieldFlags[(int)idx] & ~mod));
            if (rowFlag == ERowFlag.Modified)
            {
                if (f) RowFlags |= ERowFlag.Modified;
                else
                {
                    bool hasModification = false;
                    foreach (EFieldFlag ff in FieldFlags) { if ((ff & EFieldFlag.Modified) != 0) { hasModification = true; break; } }
                    if (!hasModification) RowFlags &= ~ERowFlag.Modified;
                }
                Data.CountModification(f ? 1 : -1);
            }
            else
            {
                RowFlags = (f ? (RowFlags | rowFlag) : (RowFlags & ~rowFlag));
                Data.CountWarning(f ? 1 : -1);
            }
        }

        void Set(EFieldIndices idx, ref string val, string orgval, string newval)
        {
            if (String.IsNullOrWhiteSpace(newval)) newval = null;
            FlagModified(idx, !String.Equals(newval, orgval, StringComparison.Ordinal));
            FlagWarning(idx, false);
            val = newval;
            SearchCache = null;
        }

        void Set(EFieldIndices idx, ref uint val, uint orgval, uint newval)
        {
            FlagModified(idx, (newval != orgval));
            FlagWarning(idx, false);
            val = newval;
            SearchCache = null;
        }

        void Set(EFieldIndices idx, ref bool val, bool orgval, bool newval)
        {
            FlagModified(idx, (newval != orgval));
            FlagWarning(idx, false);
            val = newval;
            SearchCache = null;
        }

        void Set(EFieldIndices idx, ref DateTime val, DateTime orgval, DateTime newval)
        {
            FlagModified(idx, (newval != orgval));
            FlagWarning(idx, false);
            val = newval;
            SearchCache = null;
        }

        static string FixHex(string hex, int len)
        {
            if (String.IsNullOrWhiteSpace(hex)) return null;
            hex = hex.ToUpperInvariant();
            for (int i = 0; i != hex.Length; i++)
                if (hex[i] < '0' || (hex[i] > '9' && hex[i] < 'A') || hex[i] > 'F')
                    hex = hex.Remove(i--, 1);
            if (hex.Length > len) return hex.Substring(0, len);
            while (hex.Length < len) hex += '0';
            return hex;
        }

        public void ClearWarnings()
        {
            if ((RowFlags & ERowFlag.WarningsMask) != 0)
                for (int i = 0; i != (int)EFieldIndices.COUNT; i++)
                    FlagWarning((EFieldIndices)i, false);
        }

        public void SetOrg()
        {
            OrgName          = Name          = (string.IsNullOrWhiteSpace(Name          ) ? null : Name            );
            OrgDescription   = Description   = (string.IsNullOrWhiteSpace(Description   ) ? null : Description     );
            OrgGenre         = Genre         = (string.IsNullOrWhiteSpace(Genre         ) ? null : Genre           );
            OrgRomName       = RomName       = (string.IsNullOrWhiteSpace(RomName       ) ? null : RomName         );
            OrgSize          = Size;
            OrgUsers         = Users;
            OrgRelease       = Release;
            OrgRumble        = Rumble;
            OrgAnalog        = Analog;
            OrgCoop          = Coop;
            OrgEnhancementHW = EnhancementHW = (string.IsNullOrWhiteSpace(EnhancementHW ) ? null : EnhancementHW   );
            OrgFranchise     = Franchise     = (string.IsNullOrWhiteSpace(Franchise     ) ? null : Franchise       );
            OrgOriginalTitle = OriginalTitle = (string.IsNullOrWhiteSpace(OriginalTitle ) ? null : OriginalTitle   );
            OrgDeveloper     = Developer     = (string.IsNullOrWhiteSpace(Developer     ) ? null : Developer       );
            OrgPublisher     = Publisher     = (string.IsNullOrWhiteSpace(Publisher     ) ? null : Publisher       );
            OrgOrigin        = Origin        = (string.IsNullOrWhiteSpace(Origin        ) ? null : Origin          );
            OrgRegion        = Region        = (string.IsNullOrWhiteSpace(Region        ) ? null : Region          );
            OrgTags          = Tags          = (string.IsNullOrWhiteSpace(Tags          ) ? null : Tags            );
            OrgCRC           = CRC           = (string.IsNullOrWhiteSpace(CRC           ) ? null : FixHex(CRC,  8) );
            OrgMD5           = MD5           = (string.IsNullOrWhiteSpace(MD5           ) ? null : FixHex(MD5,  32));
            OrgSHA1          = SHA1          = (string.IsNullOrWhiteSpace(SHA1          ) ? null : FixHex(SHA1, 40));
            OrgSerial        = Serial        = (string.IsNullOrWhiteSpace(Serial        ) ? null : Serial          );
            RowFlags |= ERowFlag.FromFile;
        }

        void Cache()
        {
            SearchCache = 
                (Name          != null   ? Name               : "") + "\0" +
                (Description   != null   ? Description        : "") + "\0" +
                (Genre         != null   ? Genre              : "") + "\0" +
                (RomName       != null   ? RomName            : "") + "\0" +
                (Size               != 0 ? Size.ToString()    : "") + "\0" +
                (Users              != 0 ? Users.ToString()   : "") + "\0" +
                (Release.ToBinary() != 0 ? Release.ToString() : "") + "\0" +
                (Rumble                  ? "Rumble"           : "") + "\0" +
                (Analog                  ? "Analog"           : "") + "\0" +
                (Coop                    ? "Coop"             : "") + "\0" +
                (EnhancementHW != null   ? EnhancementHW      : "") + "\0" +
                (Franchise     != null   ? Franchise          : "") + "\0" +
                (OriginalTitle != null   ? OriginalTitle      : "") + "\0" +
                (Developer     != null   ? Developer          : "") + "\0" +
                (Publisher     != null   ? Publisher          : "") + "\0" +
                (Origin        != null   ? Origin             : "") + "\0" +
                (Region        != null   ? Region             : "") + "\0" +
                (Tags          != null   ? Tags               : "") + "\0" +
                (CRC           != null   ? CRC                : "") + "\0" +
                (MD5           != null   ? MD5                : "") + "\0" +
                (SHA1          != null   ? SHA1               : "") + "\0" +
                (Serial        != null   ? Serial             : "") + "\0";
        }

        public bool Filter(string s)
        {
            if (SearchCache == null) Cache();
            return SearchCache.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public void SetDate(int releaseDay = 0, int releaseMonth = 0, int releaseYear = 0)
        {
            if (releaseDay != 0 || releaseMonth != 0 || releaseYear != 0)
                Release = new DateTime(
                    releaseYear < 1900 || releaseYear > 2100 ? 1900 : releaseYear,
                    releaseMonth < 1 || releaseMonth > 12 ? 1 : releaseMonth,
                    releaseDay < 1 || releaseDay > 31 ? 1 : releaseDay);
        }

        static EFieldFlag MergeString(ref string str, string other, bool merge)
        {
            if (string.IsNullOrWhiteSpace(other)) return EFieldFlag.None;
            if (string.IsNullOrWhiteSpace(str))
            {
                str = other;
                return EFieldFlag.Modified;
            }
            if (str.Equals(other, StringComparison.Ordinal)) return EFieldFlag.None;
            if (!merge)
            {
                str = other;
                return EFieldFlag.Modified;
            }
            if (str.IndexOf('|') == -1 && other.IndexOf('|') == -1)
            {
                str = other + "|" + str;
                return EFieldFlag.Modified | EFieldFlag.Warning;
            }
            string[] strs = str.Split('|'), others = other.Split('|');
            bool same = true;
            for (int i = 0; i != strs.Length; i++)
                if (Array.IndexOf<string>(others, strs[i]) == -1)
                    { same = false; break; }
            if (same) return EFieldFlag.None;
            str = other;
            for (int i = 0; i != strs.Length; i++)
                if (Array.IndexOf<string>(others, strs[i]) == -1)
                    str += "|" + strs[i];
            return EFieldFlag.Modified | EFieldFlag.Warning;
        }

        public void Import(Entry b, bool metaOnly, bool merge, ERowFlag warningType)
        {
            EFieldFlag c, all = EFieldFlag.None;
            if (!metaOnly && (c = MergeString(ref Name,        b.Name,        merge)) != EFieldFlag.None) { FieldFlags[0] |= c; all |= c; }
            if (     true && (c = MergeString(ref Description, b.Description, merge)) != EFieldFlag.None) { FieldFlags[1] |= c; all |= c; }
            if (     true && (c = MergeString(ref Genre,       b.Genre,       merge)) != EFieldFlag.None) { FieldFlags[2] |= c; all |= c; }
            if (!metaOnly && (c = MergeString(ref RomName,     b.RomName,     merge)) != EFieldFlag.None) { FieldFlags[3] |= c; all |= c; }
            if (!metaOnly && b.Size != 0 && Size != b.Size)
            {
                c = EFieldFlag.Modified | (merge && Size != 0 ? EFieldFlag.Warning : EFieldFlag.None);
                Size = b.Size;
                FieldFlags[4] |= c; all |= c;
            }
            if (b.Users != 0 && Users != b.Users)
            {
                c = EFieldFlag.Modified | (merge && Users != 0 ? EFieldFlag.Warning : EFieldFlag.None);
                Users = b.Users;
                FieldFlags[5] |= c; all |= c;
            }
            if (b.Release.ToBinary() != 0 && Release.ToBinary() != b.Release.ToBinary())
            {
                c = EFieldFlag.Modified | (merge && Release.ToBinary() != 0 ? EFieldFlag.Warning : EFieldFlag.None);
                Release = b.Release;
                FieldFlags[6] |= c; all |= c;
            }
            if (!Rumble && b.Rumble) { Rumble = b.Rumble; c = EFieldFlag.Modified; FieldFlags[7] |= c; all |= c; }
            if (!Analog && b.Analog) { Analog = b.Analog; c = EFieldFlag.Modified; FieldFlags[8] |= c; all |= c; }
            if (!Coop   && b.Coop  ) { Coop   = b.Coop;   c = EFieldFlag.Modified; FieldFlags[9] |= c; all |= c; }
            if (     true && (c = MergeString(ref EnhancementHW,  b.EnhancementHW, merge)) != EFieldFlag.None) { FieldFlags[10] |= c; all |= c; }
            if (     true && (c = MergeString(ref Franchise,      b.Franchise,     merge)) != EFieldFlag.None) { FieldFlags[11] |= c; all |= c; }
            if (     true && (c = MergeString(ref OriginalTitle,  b.OriginalTitle, merge)) != EFieldFlag.None) { FieldFlags[12] |= c; all |= c; }
            if (     true && (c = MergeString(ref Developer,      b.Developer,     merge)) != EFieldFlag.None) { FieldFlags[13] |= c; all |= c; }
            if (     true && (c = MergeString(ref Publisher,      b.Publisher,     merge)) != EFieldFlag.None) { FieldFlags[14] |= c; all |= c; }
            if (     true && (c = MergeString(ref Origin,         b.Origin,        merge)) != EFieldFlag.None) { FieldFlags[15] |= c; all |= c; }
            if (     true && (c = MergeString(ref Region,         b.Region,        merge)) != EFieldFlag.None) { FieldFlags[16] |= c; all |= c; }
            if (     true && (c = MergeString(ref Tags,           b.Tags,          merge)) != EFieldFlag.None) { FieldFlags[17] |= c; all |= c; }
            if (!metaOnly && (c = MergeString(ref CRC,            b.CRC,           merge)) != EFieldFlag.None) { FieldFlags[18] |= c; all |= c; }
            if (!metaOnly && (c = MergeString(ref MD5,            b.MD5,           merge)) != EFieldFlag.None) { FieldFlags[19] |= c; all |= c; }
            if (!metaOnly && (c = MergeString(ref SHA1,           b.SHA1,          merge)) != EFieldFlag.None) { FieldFlags[20] |= c; all |= c; }
            if (!metaOnly && (c = MergeString(ref Serial,         b.Serial,        merge)) != EFieldFlag.None) { FieldFlags[21] |= c; all |= c; }
            if ((all & EFieldFlag.Modified) != 0) RowFlags |= ERowFlag.Modified;
            if ((all & EFieldFlag.Warning)  != 0) RowFlags |= warningType;
            if ((b.RowFlags & warningType) != 0)
            {
                RowFlags |= warningType;
                for (int i = 0; i != FieldFlags.Length; i++)
                    FieldFlags[i] |= (b.FieldFlags[i] & EFieldFlag.Warning);
            }
        }

        public void SetRegionAndTags(string region, string tags)
        {
            EFieldFlag c, all = EFieldFlag.None;
            if (region != null && (c = (MergeString(ref Region, region, true)                      )) != EFieldFlag.None) { FieldFlags[(int)EFieldIndices.Region] |= c; all |= c; }
            if (tags   != null && (c = (MergeString(ref Tags,   tags,   true) & EFieldFlag.Modified)) != EFieldFlag.None) { FieldFlags[(int)EFieldIndices.Tags  ] |= c; all |= c; }
            if ((all & EFieldFlag.Modified) != 0) RowFlags |= ERowFlag.Modified;
            if ((all & EFieldFlag.Warning)  != 0) RowFlags |= ERowFlag.WarningToolMergeError;
        }

        public void Revert(string propertyName)
        {
            System.Reflection.PropertyInfo piVal = typeof(Entry).GetProperty(propertyName);
            System.Reflection.FieldInfo fiOrg = typeof(Entry).GetField("Org" + propertyName.Substring(3));
            piVal.SetValue(this, fiOrg.GetValue(this));
        }
    }

    static class DAT
    {
        public static string HeaderName, HeaderDescription, HeaderVersion, HeaderHomepage;
        public static bool SerialInRom;
    }

    class EntryList : List<Entry>, System.ComponentModel.IBindingList
    {
        public class SortPropDesc : System.ComponentModel.PropertyDescriptor
        {
            public SortPropDesc(string name) : base(name, null) { }
            public override bool CanResetValue(object c) { return false; }
            public override Type ComponentType { get { return null; } }
            public override object GetValue(object c) { return null; }
            public override bool IsReadOnly { get { return false; } }
            public override Type PropertyType { get { return null; } }
            public override void ResetValue(object c) { }
            public override void SetValue(object c, object v) { }
            public override bool ShouldSerializeValue(object c) { return false; }
        }

        public SortPropDesc SortProp;
        public bool SortDesc;
        public bool AllowEdit                  { get { return true; } }
        public bool IsSorted                   { get { return (SortProp != null); } }
        public bool SupportsChangeNotification { get { return true; } }
        public bool SupportsSorting            { get { return true; } }
        public bool SupportsSearching          { get { return false; } }
        public bool AllowNew                   { get { return false; } }
        public bool AllowRemove                { get { return false; } }
        public System.ComponentModel.ListSortDirection SortDirection { get { return (SortDesc ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending); } }
        public System.ComponentModel.PropertyDescriptor SortProperty { get { return SortProp; } }
        public void AddIndex(System.ComponentModel.PropertyDescriptor property) { }
        public object AddNew() { return null; }
        public void ApplySort(System.ComponentModel.PropertyDescriptor property, System.ComponentModel.ListSortDirection direction) { }
        public int Find(System.ComponentModel.PropertyDescriptor property, object key) { return 0; }
        public void RemoveIndex(System.ComponentModel.PropertyDescriptor property) { }
        public void RemoveSort() { }
        public event System.ComponentModel.ListChangedEventHandler ListChanged;
        public void BroadcastListChanged() { if (ListChanged != null) ListChanged(this, new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.Reset, 0)); }

        enum MsgPackType : byte
        {
            FIXMAP    = 0x80, FIXARRAY  = 0x90, FIXSTR    = 0xa0,
            NIL       = 0xc0, FALSE     = 0xc2, TRUE      = 0xc3,
            BIN8      = 0xc4, BIN16     = 0xc5, BIN32     = 0xc6,
            UINT8     = 0xcc, UINT16    = 0xcd, UINT32    = 0xce, UINT64    = 0xcf,
            INT8      = 0xd0, INT16     = 0xd1, INT32     = 0xd2, INT64     = 0xd3,
            STR8      = 0xd9, STR16     = 0xda, STR32     = 0xdb,
            ARRAY16   = 0xdc, ARRAY32   = 0xdd,
            MAP16     = 0xde, MAP32     = 0xdf,
        };

        enum MsgPackMode : byte { Map, Array, String, Bin, Boolean, Signed, Unsigned, Nil };

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)] struct MsgPackValue
        {
            [System.Runtime.InteropServices.FieldOffset(0)]  public MsgPackMode mode;
            [System.Runtime.InteropServices.FieldOffset(8)]  public UInt64 uint64;
            [System.Runtime.InteropServices.FieldOffset(8)]  public Int64 int64;
            [System.Runtime.InteropServices.FieldOffset(8)]  public bool b;
            [System.Runtime.InteropServices.FieldOffset(8)]  public int offset;
            [System.Runtime.InteropServices.FieldOffset(8)]  public int obj;
            [System.Runtime.InteropServices.FieldOffset(12)] public int len;
            public string UTF8String(byte[] rdb)  { return System.Text.Encoding.UTF8.GetString(rdb, offset, len); }
            public string ASCIIString(byte[] rdb) { return System.Text.Encoding.ASCII.GetString(rdb, offset, len); }
            public string HEXString(byte[] rdb)   { return BitConverter.ToString(rdb, offset, len).Replace("-", ""); }
        }

        static MsgPackValue rmsgpack_read(byte[] rdb, List<object> objs, ref int off)
        {
            byte type = rdb[off++];
            MsgPackValue res = default(MsgPackValue);
            UInt64 len;
            switch (type)
            {
                case (byte)MsgPackType.NIL:   res.mode = MsgPackMode.Nil; break;
                case (byte)MsgPackType.FALSE: res.mode = MsgPackMode.Boolean; res.b = false; break;
                case (byte)MsgPackType.TRUE:  res.mode = MsgPackMode.Boolean; res.b = true;  break;
                case (byte)MsgPackType.STR8:  len = rdb[off++]; goto READ_STRING;
                case (byte)MsgPackType.STR16: len = BitConverter.ToUInt16(rdb, off).FromBigEndian(); off += 2; goto READ_STRING;
                case (byte)MsgPackType.STR32: len = BitConverter.ToUInt32(rdb, off).FromBigEndian(); off += 4; goto READ_STRING;
                READ_STRING:
                    res.mode = MsgPackMode.String;
                    res.offset = off;
                    res.len = (int)len;
                    off += (int)len;
                    break;
                case (byte)MsgPackType.BIN8:  len = rdb[off++]; goto READ_BIN;
                case (byte)MsgPackType.BIN16: len = BitConverter.ToUInt16(rdb, off).FromBigEndian(); off += 2; goto READ_BIN;
                case (byte)MsgPackType.BIN32: len = BitConverter.ToUInt32(rdb, off).FromBigEndian(); off += 4; goto READ_BIN;
                READ_BIN:
                    res.mode = MsgPackMode.Bin;
                    res.offset = off;
                    res.len = (int)len;
                    off += (int)len;
                    break;
                case (byte)MsgPackType.UINT8:  res.mode = MsgPackMode.Unsigned; res.uint64 = rdb[off++]; break;
                case (byte)MsgPackType.UINT16: res.mode = MsgPackMode.Unsigned; res.uint64 = BitConverter.ToUInt16(rdb, off).FromBigEndian(); off += 2; break;
                case (byte)MsgPackType.UINT32: res.mode = MsgPackMode.Unsigned; res.uint64 = BitConverter.ToUInt32(rdb, off).FromBigEndian(); off += 4; break;
                case (byte)MsgPackType.UINT64: res.mode = MsgPackMode.Unsigned; res.uint64 = BitConverter.ToUInt64(rdb, off).FromBigEndian(); off += 8; break;
                case (byte)MsgPackType.INT8:   res.mode = MsgPackMode.Signed;   res.int64  = (sbyte)rdb[off++]; break;
                case (byte)MsgPackType.INT16:  res.mode = MsgPackMode.Signed;   res.int64  = BitConverter.ToInt16(rdb, off).FromBigEndian(); off += 2; break;
                case (byte)MsgPackType.INT32:  res.mode = MsgPackMode.Signed;   res.int64  = BitConverter.ToInt32(rdb, off).FromBigEndian(); off += 4; break;
                case (byte)MsgPackType.INT64:  res.mode = MsgPackMode.Signed;   res.int64  = BitConverter.ToInt64(rdb, off).FromBigEndian(); off += 8; break;
                case (byte)MsgPackType.ARRAY16: len = BitConverter.ToUInt16(rdb, off).FromBigEndian(); off += 2; goto READ_ARRAY;
                case (byte)MsgPackType.ARRAY32: len = BitConverter.ToUInt32(rdb, off).FromBigEndian(); off += 4; goto READ_ARRAY;
                READ_ARRAY:
                    MsgPackValue[] arr = new MsgPackValue[len];
                    res.mode = MsgPackMode.Array;
                    res.obj = objs.Count;
                    objs.Add(arr);
                    for (UInt64 i = 0; i != len; i++)
                        arr[i] = rmsgpack_read(rdb, objs, ref off);
                    break;
                case (byte)MsgPackType.MAP16: len = BitConverter.ToUInt16(rdb, off).FromBigEndian(); off += 2; goto READ_MAP;
                case (byte)MsgPackType.MAP32: len = BitConverter.ToUInt32(rdb, off).FromBigEndian(); off += 4; goto READ_MAP;
                READ_MAP:
                    Dictionary<string, MsgPackValue> map = new Dictionary<string, MsgPackValue>((int)len);
                    res.mode = MsgPackMode.Map;
                    res.obj = objs.Count;
                    objs.Add(map);
                    for (UInt64 i = 0; i != len; i++)
                    {
                        MsgPackValue key = rmsgpack_read(rdb, objs, ref off);
                        if (key.mode != MsgPackMode.String) throw new Exception("RDB Key must be a string");
                        MsgPackValue val = rmsgpack_read(rdb, objs, ref off);
                        try { map.Add(string.Intern(key.UTF8String(rdb)), val); } catch { }
                    }
                    break;
                default:
                    if      (type < (byte)MsgPackType.FIXMAP)   { res.mode = MsgPackMode.Signed; res.int64 = type; }
                    else if (type < (byte)MsgPackType.FIXARRAY) { len = (UInt64)(type - (byte)MsgPackType.FIXMAP);   goto READ_MAP;    }
                    else if (type < (byte)MsgPackType.FIXSTR)   { len = (UInt64)(type - (byte)MsgPackType.FIXARRAY); goto READ_ARRAY;  }
                    else if (type < (byte)MsgPackType.NIL)      { len = (UInt64)(type - (byte)MsgPackType.FIXSTR);   goto READ_STRING; }
                    else if (type > (byte)MsgPackType.MAP32)    { res.mode = MsgPackMode.Signed; res.int64 = (int)type - 0xff - 1; }
                    break;
            }
            return res;
        }

        void LoadRDB(string rdbPath, bool isInit)
        {
            List<object> objs = new List<object>();
            byte[] rdb = File.ReadAllBytes(rdbPath);
            UInt64 magic = BitConverter.ToUInt64(rdb, 0).FromBigEndian();
            if (magic != 0x5241524348444200) throw new Exception("File is not in RDB format");
            UInt64 offset = BitConverter.ToUInt64(rdb, 8).FromBigEndian();
            int off = (int)offset;
            Dictionary<string, MsgPackValue> meta = (Dictionary<string, MsgPackValue>)objs[rmsgpack_read(rdb, objs, ref off).obj];
            Int64 count = meta["count"].int64;

            string rdbKeyName           = string.Intern("name"          );
            string rdbKeyDescription    = string.Intern("description"   );
            string rdbKeyGenre          = string.Intern("genre"         );
            string rdbKeyRomName        = string.Intern("rom_name"      );
            string rdbKeySize           = string.Intern("size"          );
            string rdbKeyUsers          = string.Intern("users"         );
            string rdbKeyReleaseDay     = string.Intern("releaseday"    );
            string rdbKeyReleaseMonth   = string.Intern("releasemonth"  );
            string rdbKeyReleaseYear    = string.Intern("releaseyear"   );
            string rdbKeyRumble         = string.Intern("rumble"        );
            string rdbKeyAnalog         = string.Intern("analog"        );
            string rdbKeyCoop           = string.Intern("coop"          );
            string rdbKeyEnhancementHW  = string.Intern("enhancement_hw");
            string rdbKeyFranchise      = string.Intern("franchise"     );
            string rdbKeyOriginalTitle  = string.Intern("original_title");
            string rdbKeyDeveloper      = string.Intern("developer"     );
            string rdbKeyPublisher      = string.Intern("publisher"     );
            string rdbKeyOrigin         = string.Intern("origin"        );
            string rdbKeyRegion         = string.Intern("region"        );
            string rdbKeyTags           = string.Intern("tags"          );
            string rdbKeyCRC            = string.Intern("crc"           );
            string rdbKeyMD5            = string.Intern("md5"           );
            string rdbKeySHA1           = string.Intern("sha1"          );
            string rdbKeySerial         = string.Intern("serial"        );

            off = 16;
            MsgPackValue v;
            for (Int64 i = 0; i != count; i++)
            {
                Dictionary<string, MsgPackValue> row = (Dictionary<string, MsgPackValue>)objs[rmsgpack_read(rdb, objs, ref off).obj];
                Entry e = new Entry();
                int releaseDay = 0, releaseMonth = 0, releaseYear = 0;
                if (row.TryGetValue(rdbKeyName         , out v) && v.mode == MsgPackMode.String  ) e.Name          = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyDescription  , out v) && v.mode == MsgPackMode.String  ) e.Description   = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyGenre        , out v) && v.mode == MsgPackMode.String  ) e.Genre         = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyRomName      , out v) && v.mode == MsgPackMode.String  ) e.RomName       = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeySize         , out v) && v.mode == MsgPackMode.Unsigned) e.Size          = (uint)v.uint64;
                if (row.TryGetValue(rdbKeyUsers        , out v) && v.mode == MsgPackMode.Unsigned) e.Users         = (uint)v.uint64;
                if (row.TryGetValue(rdbKeyReleaseDay   , out v) && v.mode == MsgPackMode.Unsigned) releaseDay      = (int)v.uint64;
                if (row.TryGetValue(rdbKeyReleaseMonth , out v) && v.mode == MsgPackMode.Unsigned) releaseMonth    = (int)v.uint64;
                if (row.TryGetValue(rdbKeyReleaseYear  , out v) && v.mode == MsgPackMode.Unsigned) releaseYear     = (int)v.uint64;
                if (row.TryGetValue(rdbKeyRumble       , out v) && v.mode == MsgPackMode.Unsigned) e.Rumble        = v.uint64 != 0;
                if (row.TryGetValue(rdbKeyAnalog       , out v) && v.mode == MsgPackMode.Unsigned) e.Analog        = v.uint64 != 0;
                if (row.TryGetValue(rdbKeyCoop         , out v) && v.mode == MsgPackMode.Unsigned) e.Coop          = v.uint64 != 0;
                if (row.TryGetValue(rdbKeyEnhancementHW, out v) && v.mode == MsgPackMode.String  ) e.EnhancementHW = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyFranchise    , out v) && v.mode == MsgPackMode.String  ) e.Franchise     = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyOriginalTitle, out v) && v.mode == MsgPackMode.String  ) e.OriginalTitle = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyDeveloper    , out v) && v.mode == MsgPackMode.String  ) e.Developer     = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyPublisher    , out v) && v.mode == MsgPackMode.String  ) e.Publisher     = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyOrigin       , out v) && v.mode == MsgPackMode.String  ) e.Origin        = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyRegion       , out v) && v.mode == MsgPackMode.String  ) e.Region        = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyTags         , out v) && v.mode == MsgPackMode.String  ) e.Tags          = v.UTF8String(rdb);
                if (row.TryGetValue(rdbKeyCRC          , out v) && v.mode == MsgPackMode.Bin     ) e.CRC           = v.HEXString(rdb);
                if (row.TryGetValue(rdbKeyMD5          , out v) && v.mode == MsgPackMode.Bin     ) e.MD5           = v.HEXString(rdb);
                if (row.TryGetValue(rdbKeySHA1         , out v) && v.mode == MsgPackMode.Bin     ) e.SHA1          = v.HEXString(rdb);
                if (row.TryGetValue(rdbKeySerial       , out v) && v.mode == MsgPackMode.Bin     ) e.Serial        = v.ASCIIString(rdb);
                e.SetDate(releaseDay, releaseMonth, releaseYear);
                if (isInit) e.SetOrg();
                Add(e);
            }
        }

        void LoadDAT(string datPath, bool isInit)
        {
            string dat = File.ReadAllText(datPath, System.Text.Encoding.UTF8).Replace("\r", "");
            Regex rexCombineSpace = new Regex(@"[\r\n\s]+", RegexOptions.Singleline);
            Regex rexNextKey      = new Regex(@"\G\s*(\)|[a-z0-9_]*)");
            Regex rexNextHex      = new Regex(@"\G\s*([0-9A-Fa-f]+) ");
            Regex rexNextString   = new Regex(@"\G\s*\""(.*?(?<!\\)(?:(\\\\)*))[\""]");
            Regex rexNextNumeric  = new Regex(@"\G\s*(\d+)");
            Regex rexNextObject   = new Regex(@"\G\s*\(");

            string datKeyName           = string.Intern("name");
            string datKeyDescription    = string.Intern("description");
            string datKeyGenre          = string.Intern("genre");
            string datKeySize           = string.Intern("size");
            string datKeyUsers          = string.Intern("users");
            string datKeyReleaseDay     = string.Intern("releaseday");
            string datKeyReleaseMonth   = string.Intern("releasemonth");
            string datKeyReleaseYear    = string.Intern("releaseyear");
            string datKeyRumble         = string.Intern("rumble");
            string datKeyAnalog         = string.Intern("analog");
            string datKeyEnhancementHW  = string.Intern("enhancement_hw");
            string datKeyFranchise      = string.Intern("franchise");
            string datKeyOriginalTitle  = string.Intern("original_title");
            string datKeyDeveloper      = string.Intern("developer");
            string datKeyPublisher      = string.Intern("publisher");
            string datKeyOrigin         = string.Intern("origin");
            string datKeyRegion         = string.Intern("region");
            string datKeyTags           = string.Intern("tags");
            string datKeyCoop           = string.Intern("coop");
            string datKeyCRC            = string.Intern("crc");
            string datKeyMD5            = string.Intern("md5");
            string datKeySHA1           = string.Intern("sha1");
            string datKeySerial         = string.Intern("serial");
            string datKeyVersion        = string.Intern("version");
            string datKeyHomepage       = string.Intern("homepage");

            string game = null;
            int off = 0;
            List<Dictionary<string, string>> roms = new List<Dictionary<string,string>>();

            System.Func<int, Dictionary<string, string>> ParseDATObject = null;
            ParseDATObject = (int depth) =>
            {
                Dictionary<string, string> obj = new Dictionary<string,string>();
                for (;;)
                {
                    Match mKey = rexNextKey.Match(game, off), mVal;
                    off += mKey.Groups[0].Length;
                    string key = String.Intern(mKey.Groups[1].Value);
                    if (key.Equals(")") || key.Equals(""))
                    {
                        if (depth == 0 && off != game.Length) {  throw new Exception("Invalid string in DAT entry [" + game + "]"); }
                        return obj;
                    }
                    else if (key.Equals("crc") || key.Equals("md5") || key.Equals("sha1"))
                    {
                        Match mHex = rexNextHex.Match(game, off);
                        off += mHex.Groups[0].Length;
                        obj.Add(key, mHex.Groups[1].Value);
                    }
                    else if ((mVal = rexNextString.Match(game, off)).Success)
                    {
                        off += mVal.Groups[0].Length;
                        obj.Add(key, mVal.Groups[1].Value);
                    }
                    else if ((mVal = rexNextNumeric.Match(game, off)).Success)
                    {
                        off += mVal.Groups[0].Length;
                        obj.Add(key, mVal.Groups[1].Value);
                    }
                    else if ((mVal = rexNextObject.Match(game, off)).Success && key.Equals("rom"))
                    {
                        off += mVal.Groups[0].Length;
                        roms.Add(ParseDATObject(depth + 1));
                    }
                    else
                    {
                        throw new Exception("Could not parse value of key " + key + " in DAT entry [" + game + "]");
                    }
                }
            };

            if (isInit)
            {
                foreach (Match m in Regex.Matches(dat, @"clrmamepro\s*\((.*?)\n+\)", RegexOptions.Singleline))
                {
                    game = rexCombineSpace.Replace(m.Groups[1].Value, " ");
                    off = 0;
                    Dictionary<string, string> obj = ParseDATObject(0);
                    string v;
                    if (obj.TryGetValue(datKeyName,        out v)) DAT.HeaderName = v;
                    if (obj.TryGetValue(datKeyDescription, out v)) DAT.HeaderDescription = v;
                    if (obj.TryGetValue(datKeyVersion,     out v)) DAT.HeaderVersion = v;
                    if (obj.TryGetValue(datKeyHomepage,    out v)) DAT.HeaderHomepage = v;
                }
                if (game == null) throw new Exception("DAT file is missing clrmamepro header");
            }

            foreach (Match m in Regex.Matches(dat, @"\ngame\s*\((.*?)\n+\)", RegexOptions.Singleline))
            {
                game = rexCombineSpace.Replace(m.Groups[1].Value, " ");
                roms.Clear();
                off = 0;
                Dictionary<string, string> obj = ParseDATObject(0);
                if (roms.Count == 0) roms.Add(new Dictionary<string, string>());
                foreach (Dictionary<string, string> rom in roms)
                {
                    Entry e = new Entry();
                    int releaseDay = 0, releaseMonth = 0, releaseYear = 0;
                    string v;
                    if (obj.TryGetValue(datKeyName         , out v)) e.Name          = v;
                    if (obj.TryGetValue(datKeyDescription  , out v)) e.Description   = v;
                    if (obj.TryGetValue(datKeyGenre        , out v)) e.Genre         = v;
                    if (rom.TryGetValue(datKeyName         , out v)) e.RomName       = v;
                    if (rom.TryGetValue(datKeySize         , out v)) e.Size          = Convert.ToUInt32(v);
                    if (obj.TryGetValue(datKeyUsers        , out v)) e.Users         = Convert.ToUInt32(v);
                    if (obj.TryGetValue(datKeyReleaseDay   , out v)) releaseDay      = Convert.ToInt32(v);
                    if (obj.TryGetValue(datKeyReleaseMonth , out v)) releaseMonth    = Convert.ToInt32(v);
                    if (obj.TryGetValue(datKeyReleaseYear  , out v)) releaseYear     = Convert.ToInt32(v);
                    if (obj.TryGetValue(datKeyRumble       , out v)) e.Rumble        = Convert.ToInt32(v) != 0;
                    if (obj.TryGetValue(datKeyAnalog       , out v)) e.Analog        = Convert.ToInt32(v) != 0;
                    if (obj.TryGetValue(datKeyCoop         , out v)) e.Coop          = Convert.ToInt32(v) != 0;
                    if (obj.TryGetValue(datKeyEnhancementHW, out v)) e.EnhancementHW = v;
                    if (obj.TryGetValue(datKeyFranchise    , out v)) e.Franchise     = v;
                    if (obj.TryGetValue(datKeyOriginalTitle, out v)) e.OriginalTitle = v;
                    if (obj.TryGetValue(datKeyDeveloper    , out v)) e.Developer     = v;
                    if (obj.TryGetValue(datKeyPublisher    , out v)) e.Publisher     = v;
                    if (obj.TryGetValue(datKeyOrigin       , out v)) e.Origin        = v;
                    if (obj.TryGetValue(datKeyRegion       , out v)) e.Region        = v;
                    if (obj.TryGetValue(datKeyTags         , out v)) e.Tags          = v;
                    if (rom.TryGetValue(datKeyCRC          , out v)) e.CRC           = v;
                    if (rom.TryGetValue(datKeyMD5          , out v)) e.MD5           = v;
                    if (rom.TryGetValue(datKeySHA1         , out v)) e.SHA1          = v;
                    if (rom.TryGetValue(datKeySerial       , out v)) e.Serial        = v;
                    if (obj.TryGetValue(datKeySerial       , out v)){e.Serial        = v; if (isInit) DAT.SerialInRom = true; }
                    e.SetDate(releaseDay, releaseMonth, releaseYear);
                    if (isInit) e.SetOrg();
                    Add(e);
                }
            }
        }

        public void SaveRDB(string path)
        {
            BinaryWriter f = new BinaryWriter(File.OpenWrite(path), System.Text.Encoding.UTF8, false);
            f.Write(new byte[] { (byte)'R',(byte)'A',(byte)'R',(byte)'C',(byte)'H',(byte)'D',(byte)'B',0 });
            f.Write((ulong)0); //offset

            byte[] writeBuf = new byte[1024];
            Action<string> MsgPackWriteString = (string s) =>
            {
                int len = System.Text.Encoding.UTF8.GetByteCount(s);
                if      (len <   256) { f.Write((byte)MsgPackType.STR8);  f.Write((byte)len); }
                else if (len < 65536) { f.Write((byte)MsgPackType.STR16); f.Write(((ushort)len).ToBigEndian()); }
                else                  { f.Write((byte)MsgPackType.STR32); f.Write(((uint)len).ToBigEndian()); }
                if (len > writeBuf.Length) writeBuf = new byte[len];
                System.Text.Encoding.UTF8.GetBytes(s, 0, s.Length, writeBuf, 0);
                f.Write(writeBuf, 0, len);
            };
            Action<string> MsgPackWriteBinAscii = (string s) =>
            {
                int len = System.Text.Encoding.UTF8.GetByteCount(s);
                if      (len <   256) { f.Write((byte)MsgPackType.BIN8);  f.Write((byte)len); }
                else if (len < 65536) { f.Write((byte)MsgPackType.BIN16); f.Write(((ushort)len).ToBigEndian()); }
                else                  { f.Write((byte)MsgPackType.BIN32); f.Write(((uint)len).ToBigEndian()); }
                if (len > writeBuf.Length) writeBuf = new byte[len];
                System.Text.Encoding.UTF8.GetBytes(s, 0, s.Length, writeBuf, 0);
                f.Write(writeBuf, 0, len);
            };
            Action<string> MsgPackWriteBinHex = (string hex) =>
            {
                int len = hex.Length / 2;
                if      (len <   256) { f.Write((byte)MsgPackType.BIN8);  f.Write((byte)len); }
                else if (len < 65536) { f.Write((byte)MsgPackType.BIN16); f.Write(((ushort)len).ToBigEndian()); }
                else                  { f.Write((byte)MsgPackType.BIN32); f.Write(((uint)len).ToBigEndian()); }
                if (len > writeBuf.Length) writeBuf = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    int c = hex[i*2], d = hex[i*2+1]; 
                    writeBuf[i]  = (byte)(((c <= (int)'9' ? c - (int)'0' : (c <= (int)'F' ? c - ((int)'A' - 10) : c - ((int)'a' - 10))) << 4)
                                        | ((d <= (int)'9' ? d - (int)'0' : (d <= (int)'F' ? d - ((int)'A' - 10) : d - ((int)'a' - 10)))     ));
                }
                f.Write(writeBuf, 0, len);
            };
            Action<int> MsgPackWriteMap = (int count) =>
            {
                if (count < 16) f.Write((byte)((byte)MsgPackType.FIXMAP + count));
                else { f.Write((byte)MsgPackType.MAP16); f.Write(((ushort)count).ToBigEndian()); }
            };
            Action<uint> MsgPackWriteUint = (uint i) =>
            {
                if (i < (uint)MsgPackType.FIXMAP) { f.Write((byte)i); }
                else if (i <   256) { f.Write((byte)MsgPackType.UINT8);  f.Write((byte)i); }
                else if (i < 65536) { f.Write((byte)MsgPackType.UINT16); f.Write(((ushort)i).ToBigEndian()); }
                else                { f.Write((byte)MsgPackType.UINT32); f.Write(((uint)i).ToBigEndian()); }
            };

            uint entryCount = 0;
            foreach (Entry e in this)
            {
                int count = 0;
                if (!string.IsNullOrWhiteSpace(e.Name         )) count++;
                if (!string.IsNullOrWhiteSpace(e.Description  )) count++;
                if (!string.IsNullOrWhiteSpace(e.Genre        )) count++;
                if (!string.IsNullOrWhiteSpace(e.RomName      )) count++;
                if (e.Size                                 != 0) count++;
                if (e.Users                                != 0) count++;
                if (e.Release.ToBinary()                   != 0) count += 3;
                if (e.Rumble                                   ) count++;
                if (e.Analog                                   ) count++;
                if (e.Coop                                     ) count++;
                if (!string.IsNullOrWhiteSpace(e.EnhancementHW)) count++;
                if (!string.IsNullOrWhiteSpace(e.Franchise    )) count++;
                if (!string.IsNullOrWhiteSpace(e.OriginalTitle)) count++;
                if (!string.IsNullOrWhiteSpace(e.Developer    )) count++;
                if (!string.IsNullOrWhiteSpace(e.Publisher    )) count++;
                if (!string.IsNullOrWhiteSpace(e.Origin       )) count++;
                if (!string.IsNullOrWhiteSpace(e.Region       )) count++;
                if (!string.IsNullOrWhiteSpace(e.Tags         )) count++;
                if (!string.IsNullOrWhiteSpace(e.CRC          )) count++;
                if (!string.IsNullOrWhiteSpace(e.MD5          )) count++;
                if (!string.IsNullOrWhiteSpace(e.SHA1         )) count++;
                if (!string.IsNullOrWhiteSpace(e.Serial       )) count++;
                if (count == 0) continue;
                MsgPackWriteMap(count);
                if (!string.IsNullOrWhiteSpace(e.Name         ))  { MsgPackWriteString("name"          ); MsgPackWriteString(e.Name             ); }
                if (!string.IsNullOrWhiteSpace(e.Description  ))  { MsgPackWriteString("description"   ); MsgPackWriteString(e.Description      ); }
                if (!string.IsNullOrWhiteSpace(e.Genre        ))  { MsgPackWriteString("genre"         ); MsgPackWriteString(e.Genre            ); }
                if (!string.IsNullOrWhiteSpace(e.RomName      ))  { MsgPackWriteString("rom_name"      ); MsgPackWriteString(e.RomName          ); }
                if (e.Size                                 != 0)  { MsgPackWriteString("size"          ); MsgPackWriteUint(e.Size               ); }
                if (e.Users                                != 0)  { MsgPackWriteString("users"         ); MsgPackWriteUint(e.Users              ); }
                if (e.Release.ToBinary()                   != 0)  { MsgPackWriteString("releaseday"    ); MsgPackWriteUint((uint)e.Release.Day  ); }
                if (e.Release.ToBinary()                   != 0)  { MsgPackWriteString("releasemonth"  ); MsgPackWriteUint((uint)e.Release.Month); }
                if (e.Release.ToBinary()                   != 0)  { MsgPackWriteString("releaseyear"   ); MsgPackWriteUint((uint)e.Release.Year ); }
                if (e.Rumble                                   )  { MsgPackWriteString("rumble"        ); f.Write((byte)MsgPackType.TRUE);         }
                if (e.Analog                                   )  { MsgPackWriteString("analog"        ); f.Write((byte)MsgPackType.TRUE);         }
                if (e.Coop                                     )  { MsgPackWriteString("coop"          ); f.Write((byte)MsgPackType.TRUE);         }
                if (!string.IsNullOrWhiteSpace(e.EnhancementHW))  { MsgPackWriteString("enhancement_hw"); MsgPackWriteString(e.EnhancementHW    ); }
                if (!string.IsNullOrWhiteSpace(e.Franchise    ))  { MsgPackWriteString("franchise"     ); MsgPackWriteString(e.Franchise        ); }
                if (!string.IsNullOrWhiteSpace(e.OriginalTitle))  { MsgPackWriteString("original_title"); MsgPackWriteString(e.OriginalTitle    ); }
                if (!string.IsNullOrWhiteSpace(e.Developer    ))  { MsgPackWriteString("developer"     ); MsgPackWriteString(e.Developer        ); }
                if (!string.IsNullOrWhiteSpace(e.Publisher    ))  { MsgPackWriteString("publisher"     ); MsgPackWriteString(e.Publisher        ); }
                if (!string.IsNullOrWhiteSpace(e.Origin       ))  { MsgPackWriteString("origin"        ); MsgPackWriteString(e.Origin           ); }
                if (!string.IsNullOrWhiteSpace(e.Region       ))  { MsgPackWriteString("region"        ); MsgPackWriteString(e.Region           ); }
                if (!string.IsNullOrWhiteSpace(e.Tags         ))  { MsgPackWriteString("tags"          ); MsgPackWriteString(e.Tags             ); }
                if (!string.IsNullOrWhiteSpace(e.CRC          ))  { MsgPackWriteString("crc"           ); MsgPackWriteBinHex(e.CRC              ); }
                if (!string.IsNullOrWhiteSpace(e.MD5          ))  { MsgPackWriteString("md5"           ); MsgPackWriteBinHex(e.MD5              ); }
                if (!string.IsNullOrWhiteSpace(e.SHA1         ))  { MsgPackWriteString("sha1"          ); MsgPackWriteBinHex(e.SHA1             ); }
                if (!string.IsNullOrWhiteSpace(e.Serial       ))  { MsgPackWriteString("serial"        ); MsgPackWriteBinAscii(e.Serial         ); }
                entryCount++;
            }
            f.Write((byte)MsgPackType.NIL); //sentinel eof marker

            ulong metaPos = (ulong)f.Seek(0, SeekOrigin.Current);
            MsgPackWriteMap(1);
            MsgPackWriteString("count");
            MsgPackWriteUint(entryCount);
            f.Seek(8, SeekOrigin.Begin);
            f.Write(metaPos.ToBigEndian());
            f.Dispose();
        }

        public void SaveDAT(string path, bool modificationsOnly = false, string key = null, bool emptyHeader = false)
        {
            StreamWriter w = new StreamWriter(path, false, new System.Text.UTF8Encoding(false));
            w.NewLine = "\n";
            w.WriteLine("clrmamepro (");
            if (!emptyHeader)
            {
                if (!string.IsNullOrWhiteSpace(DAT.HeaderName       )) { w.WriteLine("	name \""        + DAT.HeaderName        .Replace('"', '\'') + "\""); }
                if (!string.IsNullOrWhiteSpace(DAT.HeaderDescription)) { w.WriteLine("	description \"" + DAT.HeaderDescription .Replace('"', '\'') + "\""); }
                if (!string.IsNullOrWhiteSpace(DAT.HeaderVersion    )) { w.WriteLine("	version \""     + DAT.HeaderVersion     .Replace('"', '\'') + "\""); }
                if (!string.IsNullOrWhiteSpace(DAT.HeaderHomepage   )) { w.WriteLine("	homepage \""    + DAT.HeaderHomepage    .Replace('"', '\'') + "\""); }
            }
            w.WriteLine(")");
            w.WriteLine("");
            if (modificationsOnly)
            {
                EFieldIndices keyIndex = (EFieldIndices)Enum.Parse(typeof(EFieldIndices), key);
                foreach (Entry e in this)
                {
                    if ((e.RowFlags & ERowFlag.Modified) == 0) continue;

                    string ln = "game (" + "\n";
                    Action<EFieldIndices, string, string, bool> WriteString = (EFieldIndices idx, string field, string val, bool nl) =>
                        { if (keyIndex == idx || (e.FieldFlags[(int)idx] & EFieldFlag.Modified) != 0) ln += (nl ? "	" : " ") + field + " \"" + val.Replace('"', '\'') + "\"" + (nl ? "\n" : ""); };
                    Action<EFieldIndices, string, string, bool> WriteRaw    = (EFieldIndices idx, string field, string val, bool nl) =>
                        { if (keyIndex == idx || (e.FieldFlags[(int)idx] & EFieldFlag.Modified) != 0) ln += (nl ? "	" : " ") + field + " "   + val                           + (nl ? "\n" : ""); };
                    WriteString(EFieldIndices.Name         , "name"           , e.Name                    ,  true);
                    WriteString(EFieldIndices.Description  , "description"    , e.Description             ,  true);
                    WriteString(EFieldIndices.Genre        , "genre"          , e.Genre                   ,  true);
                    WriteRaw   (EFieldIndices.Users        , "users "         , e.Users        .ToString(),  true);
                    WriteRaw   (EFieldIndices.Release      , "releaseday "    , e.Release.Day  .ToString(),  true);
                    WriteRaw   (EFieldIndices.Release      , "releasemonth "  , e.Release.Month.ToString(),  true);
                    WriteRaw   (EFieldIndices.Release      , "releaseyear "   , e.Release.Year .ToString(),  true);
                    WriteRaw   (EFieldIndices.Rumble       , "rumble"         , (e.Rumble ? "1" : "0")    ,  true);
                    WriteRaw   (EFieldIndices.Analog       , "analog"         , (e.Analog ? "1" : "0")    ,  true);
                    WriteRaw   (EFieldIndices.Coop         , "coop"           , (e.Coop   ? "1" : "0")    ,  true);
                    WriteString(EFieldIndices.EnhancementHW, "enhancement_hw" , e.EnhancementHW           ,  true);
                    WriteString(EFieldIndices.Franchise    , "franchise"      , e.Franchise               ,  true);
                    WriteString(EFieldIndices.OriginalTitle, "original_title" , e.OriginalTitle           ,  true);
                    WriteString(EFieldIndices.Developer    , "developer"      , e.Developer               ,  true);
                    WriteString(EFieldIndices.Publisher    , "publisher"      , e.Publisher               ,  true);
                    WriteString(EFieldIndices.Origin       , "origin"         , e.Origin                  ,  true);
                    WriteString(EFieldIndices.Region       , "region"         , e.Region                  ,  true);
                    WriteString(EFieldIndices.Tags         , "tags"           , e.Tags                    ,  true);
                    if (!DAT.SerialInRom)
                        WriteString(EFieldIndices.Serial   , "serial"         , e.Serial                  ,  true);
                    ln += "	rom (";
                    WriteString(EFieldIndices.RomName      , "name"           , e.RomName                 , false);
                    WriteRaw   (EFieldIndices.Size         , "size"           , e.Size         .ToString(), false);
                    WriteRaw   (EFieldIndices.MD5          , "md5"            , e.MD5                     , false);
                    WriteRaw   (EFieldIndices.CRC          , "crc"            , e.CRC                     , false);
                    WriteRaw   (EFieldIndices.SHA1         , "sha1"           , e.SHA1                    , false);
                    if (DAT.SerialInRom)
                        WriteString(EFieldIndices.Serial   , "serial"         , e.Serial                  , false);
                    ln += " )\n)\n";
                    w.Write(ln);
                }
            }
            else
            {
                foreach (Entry e in this)
                {
                    w.WriteLine("game (");
                    if (!string.IsNullOrWhiteSpace(e.Name         )) { w.WriteLine("	name \""           + e.Name          .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Description  )) { w.WriteLine("	description \""    + e.Description   .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Genre        )) { w.WriteLine("	genre \""          + e.Genre         .Replace('"', '\'') + "\""); }
                    if (e.Users                                != 0) { w.WriteLine("	users "            + e.Users         .ToString()); }
                    if (e.Release.ToBinary()                   != 0) { w.WriteLine("	releaseday "       + e.Release.Day   .ToString()); }
                    if (e.Release.ToBinary()                   != 0) { w.WriteLine("	releasemonth "     + e.Release.Month .ToString()); }
                    if (e.Release.ToBinary()                   != 0) { w.WriteLine("	releaseyear "      + e.Release.Year  .ToString()); }
                    if (e.Rumble                                   ) { w.WriteLine("	rumble 1"        ); }
                    if (e.Analog                                   ) { w.WriteLine("	analog 1"        ); }
                    if (e.Coop                                     ) { w.WriteLine("	coop 1"          ); }
                    if (!string.IsNullOrWhiteSpace(e.EnhancementHW)) { w.WriteLine("	enhancement_hw \"" + e.EnhancementHW .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Franchise    )) { w.WriteLine("	franchise \""      + e.Franchise     .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.OriginalTitle)) { w.WriteLine("	original_title \"" + e.OriginalTitle .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Developer    )) { w.WriteLine("	developer \""      + e.Developer     .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Publisher    )) { w.WriteLine("	publisher \""      + e.Publisher     .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Origin       )) { w.WriteLine("	origin \""         + e.Origin        .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Region       )) { w.WriteLine("	region \""         + e.Region        .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.Tags         )) { w.WriteLine("	tags \""           + e.Tags          .Replace('"', '\'') + "\""); }
                    if (!DAT.SerialInRom)
                        if (!string.IsNullOrWhiteSpace(e.Serial       )) { w.WriteLine("	serial \""         + e.Serial        .Replace('"', '\'') + "\""); }
                    if (!string.IsNullOrWhiteSpace(e.RomName) || e.Size != 0 || !string.IsNullOrWhiteSpace(e.MD5) || !string.IsNullOrWhiteSpace(e.CRC) || !string.IsNullOrWhiteSpace(e.SHA1) || (DAT.SerialInRom && !string.IsNullOrWhiteSpace(e.Serial)))
                    {
                        w.Write("	rom (");
                        if (!string.IsNullOrWhiteSpace(e.RomName      )) { w.Write(" name \""    + e.RomName .Replace('"', '\'') + "\""); }
                        if (e.Size                                 != 0) { w.Write(" size "      + e.Size    .ToString()); }
                        if (!string.IsNullOrWhiteSpace(e.CRC          )) { w.Write(" crc "       + e.CRC ); }
                        if (!string.IsNullOrWhiteSpace(e.MD5          )) { w.Write(" md5 "       + e.MD5 ); }
                        if (!string.IsNullOrWhiteSpace(e.SHA1         )) { w.Write(" sha1 "      + e.SHA1); }
                        if (DAT.SerialInRom)
                            if (!string.IsNullOrWhiteSpace(e.Serial   )) { w.Write(" serial \""  + e.Serial  .Replace('"', '\'') + "\""); }
                        w.WriteLine(" )");
                    }
                    w.WriteLine(")");
                }
            }
            w.Dispose();
        }

        public static EntryList Load(string path, bool isInit = false)
        {
            try
            {
                EntryList res = new EntryList();
                if (path.EndsWith(".rdb", StringComparison.OrdinalIgnoreCase)) { res.LoadRDB(path, isInit); return res; }
                if (path.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)) { res.LoadDAT(path, isInit); return res; }
                char[] buf = new char[7];
                StreamReader sr = new StreamReader(path, System.Text.Encoding.ASCII);
                try { sr.Read(buf, 0, 7); } catch { }
                sr.Dispose();
                string str = new string(buf);
                if      (str == "RARCHDB") { res.LoadRDB(path, isInit); return res; }
                else if (str == "clrmame") { res.LoadDAT(path, isInit); return res; }
                else throw new Exception("Unsupported format - Must be RDB or DAT file.");
            }
            catch (Exception e) { MessageBox.Show(e.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            return null;
        }

        public bool Import(string path, string key, bool addUnmatched, bool merge)
        {
            EntryList delta = Load(path);
            if (delta == null) return false;
            System.Reflection.FieldInfo fi = typeof(Entry).GetField(key);
            Dictionary<string, Entry> mapDelta = new Dictionary<string, Entry>();
            foreach (Entry e in delta)
            {
                object v = fi.GetValue(e); if (v == null) continue; string s = v.ToString(); if (s.Length == 0) continue;
                if (mapDelta.ContainsKey(s)) continue;
                mapDelta.Add(s, e);
            }
            Dictionary<string, Entry> mapSelf = new Dictionary<string, Entry>();
            foreach (Entry e in this)
            {
                object v = fi.GetValue(e); if (v == null) continue; string s = v.ToString(); if (s.Length == 0) continue;
                if (addUnmatched && !mapSelf.ContainsKey(s)) mapSelf.Add(s, e);
                Entry match;
                if (mapDelta.TryGetValue(s, out match))
                    e.Import(match, false, merge, ERowFlag.WarningToolMergeError);
            }
            if (addUnmatched)
            {
                foreach (Entry e in delta)
                {
                    object v = fi.GetValue(e);
                    if (v != null && mapSelf.ContainsKey(v.ToString())) continue;
                    Entry newEntry = new Entry();
                    newEntry.Import(e, false, merge, ERowFlag.WarningToolMergeError);
                    Add(newEntry);
                }
            }
            return true;
        }
    }

    static class Data
    {
        public const string DefaultNewFileName = "<New File>";
        public static string LoadedFileName = DefaultNewFileName;
        public static EntryList AllEntries = new EntryList(), Filter = new EntryList();
        public static int Modifications, Warnings, Deletions;
        public static Comparison<Entry> SortFn;

        public static void SetSortFn(string sortProp, bool desc = false)
        {
            if (sortProp == null)
            {
                Filter.SortDesc = false;
                Filter.SortProp = null;
                SortFn = null;
                return;
            }
            System.Reflection.FieldInfo fi = typeof(Entry).GetField(sortProp.Substring(3));
            if      (fi.FieldType == typeof(string)   && !desc) SortFn = (Entry a, Entry b) =>  string.Compare((string)fi.GetValue(a), (string)fi.GetValue(b), StringComparison.OrdinalIgnoreCase);
            else if (fi.FieldType == typeof(string)   &&  desc) SortFn = (Entry a, Entry b) => -string.Compare((string)fi.GetValue(a), (string)fi.GetValue(b), StringComparison.OrdinalIgnoreCase);
            else if (fi.FieldType == typeof(DateTime) && !desc) SortFn = (Entry a, Entry b) =>  DateTime.Compare((DateTime)fi.GetValue(a), (DateTime)fi.GetValue(b));
            else if (fi.FieldType == typeof(DateTime) &&  desc) SortFn = (Entry a, Entry b) => -DateTime.Compare((DateTime)fi.GetValue(a), (DateTime)fi.GetValue(b));
            else if (fi.FieldType == typeof(int)      && !desc) SortFn = (Entry a, Entry b) => (int)fi.GetValue(a)-(int)fi.GetValue(b);
            else if (fi.FieldType == typeof(int)      &&  desc) SortFn = (Entry a, Entry b) => (int)fi.GetValue(b)-(int)fi.GetValue(a);
            else if (fi.FieldType == typeof(uint)     && !desc) SortFn = (Entry a, Entry b) => (uint)fi.GetValue(a)>(uint)fi.GetValue(b)?1:(uint)fi.GetValue(a)<(uint)fi.GetValue(b)?-1:0;
            else if (fi.FieldType == typeof(uint)     &&  desc) SortFn = (Entry a, Entry b) => (uint)fi.GetValue(b)>(uint)fi.GetValue(a)?1:(uint)fi.GetValue(b)<(uint)fi.GetValue(a)?-1:0;
            else if (fi.FieldType == typeof(bool)     && !desc) SortFn = (Entry a, Entry b) => ((bool)fi.GetValue(a) == (bool)fi.GetValue(b) ? 0 : (bool)fi.GetValue(b) ? -1 : 1);
            else if (fi.FieldType == typeof(bool)     &&  desc) SortFn = (Entry a, Entry b) => ((bool)fi.GetValue(b) == (bool)fi.GetValue(a) ? 0 : (bool)fi.GetValue(a) ? -1 : 1);
            else if (fi.FieldType.IsEnum              && !desc) SortFn = (Entry a, Entry b) => (int)fi.GetValue(a)-(int)fi.GetValue(b);
            else if (fi.FieldType.IsEnum              &&  desc) SortFn = (Entry a, Entry b) => (int)fi.GetValue(b)-(int)fi.GetValue(a);
            else throw new NotImplementedException();
            Filter.SortDesc = desc;
            Filter.SortProp = new EntryList.SortPropDesc(sortProp);
        }

        enum FilterMode : byte { Contains, Equals, StartsWith, EndsWith }
        struct FilterRule { public System.Reflection.FieldInfo field; public string raw, value; public bool not, isstring; public FilterMode mode; }
        public static void SetFilter(string filter, bool onlyModifications, bool onlyWarnings)
        {
            string fieldStr = "";
            Dictionary<string, System.Reflection.FieldInfo> fieldMap = new Dictionary<string,System.Reflection.FieldInfo>();
            foreach (System.Reflection.FieldInfo fi in typeof(Entry).GetFields())
            {
                fieldStr += fi.Name + "|";
                fieldMap.Add(fi.Name.ToLower(), fi);
            }

            List<FilterRule> filterRules = new List<FilterRule>();
            Regex rexFieldFilter = new Regex(@"\s*(\??)(\!?)(" + fieldStr.TrimEnd('|') + @"):", RegexOptions.IgnoreCase);
            Regex rexQuoted = new Regex(@"\G\""(.*?(?<!\\)(?:(\\\\)*))[\""]");
            Regex rexWord = new Regex(@"\G(\S+)");
            Match m;
            bool or = false;
            while ((m = rexFieldFilter.Match(filter)).Success)
            {
                or |= (m.Groups[1].Length > 0);
                FilterRule f = new FilterRule { not = (m.Groups[2].Length > 0), field = fieldMap[m.Groups[3].Value.ToLower()], mode = FilterMode.Contains };
                int idx = m.Groups[0].Index;
                filter = filter.Remove(idx, m.Groups[0].Length);
                f.isstring = (f.field.FieldType == typeof(string));
                if (filter.Length <= idx || filter[idx] == ' ' || !(m = (filter[idx] == '"' ? rexQuoted : rexWord).Match(filter, idx)).Success)
                    continue;
                f.raw = f.value = m.Groups[1].Value.Replace("\\\"", "\"").Replace("\\\\", "\\");
                filter = filter.Remove(idx, m.Groups[0].Length);
                if (f.value.Length > 1)
                {
                    bool begin = f.value.StartsWith("|"), end = f.value.EndsWith("|");
                    if (begin && end) { f.value = f.value.Substring(1, f.value.Length - 2); f.mode = FilterMode.Equals;     }
                    else if (end)     { f.value = f.value.Substring(0, f.value.Length - 1); f.mode = FilterMode.EndsWith;   }
                    else if (begin)   { f.value = f.value.Substring(1, f.value.Length - 1); f.mode = FilterMode.StartsWith; }
                }
                filterRules.Add(f);
            }

            if (string.IsNullOrWhiteSpace(filter)) filter = null;
            Filter.Clear();
            if (filterRules.Count != 0)
            {
                FilterRule[] ruleArray = filterRules.ToArray();
                foreach (Entry e in AllEntries)
                {
                    if (filter != null && !e.Filter(filter)) continue;
                    if (onlyModifications && (e.RowFlags & ERowFlag.Modified    ) == 0) continue;
                    if (onlyWarnings      && (e.RowFlags & ERowFlag.WarningsMask) == 0) continue;
                    bool skip = or;
                    foreach (FilterRule f in ruleArray)
                    {
                        if (or != skip) break;
                        object o = f.field.GetValue(e);
                        skip = ((o == null ? (f.value.Length == 0) :
                            (f.isstring && ((string)o).IndexOf('|') >= 0 ? ("|" + (string)o + "|").IndexOf(f.raw, StringComparison.OrdinalIgnoreCase) >= 0 :
                            (f.mode == FilterMode.Equals     ? o.ToString().Equals(    f.value, StringComparison.OrdinalIgnoreCase) :
                            (f.mode == FilterMode.StartsWith ? o.ToString().StartsWith(f.value, StringComparison.OrdinalIgnoreCase) :
                            (f.mode == FilterMode.EndsWith   ? o.ToString().EndsWith(  f.value, StringComparison.OrdinalIgnoreCase) :
                            o.ToString().IndexOf(f.value, StringComparison.OrdinalIgnoreCase) >= 0)))))
                            == f.not);
                    }
                    if (!skip) Filter.Add(e);
                }
            }
            else if (filter != null)
            {
                foreach (Entry e in AllEntries)
                {
                    if (!e.Filter(filter)) continue;
                    if (onlyModifications && (e.RowFlags & ERowFlag.Modified    ) == 0) continue;
                    if (onlyWarnings      && (e.RowFlags & ERowFlag.WarningsMask) == 0) continue;
                    Filter.Add(e);
                }
            }
            else if (onlyModifications || onlyWarnings)
            {
                foreach (Entry e in AllEntries)
                {
                    if (onlyModifications && (e.RowFlags & ERowFlag.Modified    ) == 0) continue;
                    if (onlyWarnings      && (e.RowFlags & ERowFlag.WarningsMask) == 0) continue;
                    Filter.Add(e);
                }
            }
            else Filter.AddRange(AllEntries);
        }

        public static void ValidateUnique(string key)
        {
            EFieldIndices keyIndex = (EFieldIndices)Enum.Parse(typeof(EFieldIndices), key);
            System.Reflection.FieldInfo fi = typeof(Entry).GetField(key);
            Dictionary<string, int> map = new Dictionary<string, int>();
            foreach (Entry e in AllEntries)
            {
                object v = fi.GetValue(e);
                if (v == null) continue;
                string s = v.ToString();
                int i;
                map[s] = (map.TryGetValue(s, out i) ? i + 1 : 1);
            }
            foreach (Entry e in AllEntries)
            {
                if ((e.RowFlags & ERowFlag.WarningNotUnique) != 0) e.ClearWarnings();
                object v = fi.GetValue(e);
                if (v == null) continue;
                string s = v.ToString();
                if (map[s] > 1)
                    e.FlagWarning(keyIndex, true, ERowFlag.WarningNotUnique);
            }
        }

        public static void Recount()
        {
            Modifications = Warnings = 0;
            foreach (Entry e in AllEntries)
            {
                foreach (EFieldFlag fc in e.FieldFlags)
                {
                    if ((fc & EFieldFlag.Modified) != 0) Modifications++;
                    if ((fc & EFieldFlag.Warning)  != 0) Warnings++;
                }
            }
            RDBEdUI.UpdateCounts();
        }

        public static void ClearModificationFlags()
        {
            foreach (Entry e in AllEntries)
            {
                if ((e.RowFlags & ERowFlag.Modified) == 0) continue;
                for (int i = 0; i != (int)EFieldIndices.COUNT; i++)
                    e.FieldFlags[i] &= ~EFieldFlag.Modified;
                e.RowFlags &= ~ERowFlag.Modified;
                e.SetOrg();
            }
            Modifications = 0;
            RDBEdUI.UpdateCounts(true);
        }

        public static void CountModification(int delta)
        {
            Modifications += delta;
            RDBEdUI.UpdateCounts();
        }

        public static void CountWarning(int delta)
        {
            Warnings += delta;
            RDBEdUI.UpdateCounts();
        }

        public static void Init(string path)
        {
            DAT.HeaderVersion = DAT.HeaderHomepage = null;
            DAT.SerialInRom = false;
            if (path != null)
            {
                DAT.HeaderName = DAT.HeaderDescription = Path.GetFileNameWithoutExtension(path);
                EntryList newlist = EntryList.Load(path, true);
                if (newlist == null) return;
                LoadedFileName = path;
                AllEntries = newlist;
            }
            else
            {
                DAT.HeaderName = DAT.HeaderDescription = null;
                AllEntries = new EntryList();
                LoadedFileName = DefaultNewFileName;
            }
            SetSortFn(null);
            Recount();
            RDBEdUI.UpdateCounts(true);
        }

        public static void Import(string path, string key, bool addUnmatched, bool merge)
        {
            int oldCount = AllEntries.Count;
            if (!AllEntries.Import(path, key, addUnmatched, merge)) return;
            for (int i = oldCount; i != AllEntries.Count; i++)
                Filter.Add(AllEntries[i]);
            Recount();
        }

        public static void UnifyMeta(string key)
        {
            List<Entry> list = new List<Entry>(AllEntries);
            System.Reflection.FieldInfo fi = typeof(Entry).GetField(key);
            list.Sort((Entry a, Entry b) => string.CompareOrdinal((string)fi.GetValue(a), (string)fi.GetValue(b)));
            for (int i = 1, first = 0; i <= list.Count; i++)
            {
                if (i != list.Count && string.CompareOrdinal((string)fi.GetValue(list[i]), (string)fi.GetValue(list[first])) == 0)
                    continue;
                if (first + 1 == i) { first = i; continue; }
                if (string.IsNullOrEmpty((string)fi.GetValue(list[first]))) { first = i; continue; }

                for (int j = first + 1; j != i; j++) list[j].Import(list[j - 1], true, true, ERowFlag.WarningToolUnifyError);
                for (int j = i - 2; j >= first; j--) list[j].Import(list[j + 1], true, true, ERowFlag.WarningToolUnifyError);
                first = i;
            }
            Recount();
        }

        public static void GenerateDeltaDAT(string in1Path, string in2Path, string outPath, string key, bool addUnmatched)
        {
            System.Reflection.FieldInfo fi = typeof(Entry).GetField(key);
            EntryList list = EntryList.Load(in1Path);
            if (list == null || !list.Import(in2Path, key, addUnmatched, false)) return;
            list.SaveDAT(outPath, true, key);
        }

        public static bool ValidateRegex(string key, string rex, bool not)
        {
            System.Reflection.FieldInfo fi = typeof(Entry).GetField(key);
            EFieldIndices keyIndex = (EFieldIndices)Enum.Parse(typeof(EFieldIndices), key);
            try
            {
                Regex rexValid = new Regex(rex , RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                foreach (Entry e in AllEntries)
                {
                    object v = fi.GetValue(e);
                    e.FlagWarning(keyIndex, rexValid.IsMatch(v == null ? "" : v.ToString()) == not, ERowFlag.WarningInvalid);
                }
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Regular Expression Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return false; }
        }

        public static void ToolGenerateTags()
        {
            Regex rexBeta = new Regex(@" \((Beta|Beta ?\d+)\)");
            Regex rexSample = new Regex(@" \((Sample|Sample ?\d+)\)");
            Regex rexProto = new Regex(@" \((Proto|Proto ?\d+|Putative Proto)\)");
            Regex rexLanguages = new Regex(@" \(([A-Z][a-z](,[A-Z][a-z])*)\)");
            List<string> tags = new List<string>();
            foreach (Entry e in AllEntries)
            {
                if (string.IsNullOrWhiteSpace(e.Name)) continue;
                tags.Clear();
                string region = null;
                if (e.Name.IndexOf(" (Japan)"          ) != -1) region = "Japan";
                if (e.Name.IndexOf(" (USA)"            ) != -1) region = "USA";
                if (e.Name.IndexOf(" (Canada)"         ) != -1) region = "USA";
                if (e.Name.IndexOf(" (Europe)"         ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (France)"         ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (Germany)"        ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (Spain)"          ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (Sweden)"         ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (Australia)"      ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (Netherlands)"    ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (Italy)"          ) != -1) region = "Europe";
                if (e.Name.IndexOf(" (Asia)"           ) != -1) region = "Asia";
                if (e.Name.IndexOf(" (China)"          ) != -1) region = "Asia";
                if (e.Name.IndexOf(" (Hong Kong)"      ) != -1) region = "Asia";
                if (e.Name.IndexOf(" (Korea)"          ) != -1) region = "Asia";
                if (e.Name.IndexOf(" (Brazil)"         ) != -1) region = "Brazil";
                if (e.Name.IndexOf(" (Japan, USA)"     ) != -1) region = "Japan|USA";
                if (e.Name.IndexOf(" (Japan, Europe)"  ) != -1) region = "Japan|Europe";
                if (e.Name.IndexOf(" (USA, Asia)"      ) != -1) region = "USA|Asia";
                if (e.Name.IndexOf(" (USA, Europe)"    ) != -1) region = "USA|Europe";
                if (e.Name.IndexOf(" (World)"          ) != -1) region = "World";
                if (e.Name.IndexOf(" (Unl)"            ) != -1) tags.Add("Unlicensed");
                if (e.Name.IndexOf(" (Pirate)"         ) != -1) tags.Add("Pirate");
                if (e.Name.IndexOf(" (Arcade)"         ) != -1) tags.Add("Arcade");
                if (e.Name.IndexOf(" (Demo)"           ) != -1) tags.Add("Demo");
                if (rexBeta.IsMatch(e.Name)                   ) tags.Add("Beta");
                if (rexSample.IsMatch(e.Name)                 ) tags.Add("Sample");
                if (rexProto.IsMatch(e.Name)                  ) tags.Add("Prototype");
                Match m = rexLanguages.Match(e.Name);
                if (m.Success)
                {
                    string[] languages = m.Groups[1].Value.Split(',');
                    if (Array.IndexOf<string>(languages, "En") != -1) tags.Add("English");
                    if (Array.IndexOf<string>(languages, "Ja") != -1) tags.Add("Japanese");
                    if (Array.IndexOf<string>(languages, "Fr") != -1) tags.Add("French");
                    if (Array.IndexOf<string>(languages, "De") != -1) tags.Add("German");
                    if (Array.IndexOf<string>(languages, "Es") != -1) tags.Add("Spanish");
                    if (Array.IndexOf<string>(languages, "It") != -1) tags.Add("Italian");
                    if (Array.IndexOf<string>(languages, "Nl") != -1) tags.Add("Dutch");
                    if (Array.IndexOf<string>(languages, "Pt") != -1) tags.Add("Portuguese");
                    if (Array.IndexOf<string>(languages, "Sv") != -1) tags.Add("Swedish");
                    if (Array.IndexOf<string>(languages, "No") != -1) tags.Add("Norwegian");
                    if (Array.IndexOf<string>(languages, "Da") != -1) tags.Add("Danish");
                    if (Array.IndexOf<string>(languages, "Fi") != -1) tags.Add("Finish");
                    if (Array.IndexOf<string>(languages, "Zh") != -1) tags.Add("Chinese");
                    if (Array.IndexOf<string>(languages, "Ko") != -1) tags.Add("Korean");
                    if (Array.IndexOf<string>(languages, "Pl") != -1) tags.Add("Polish");
                }
                e.SetRegionAndTags(region, (tags.Count == 0 ? null : String.Join("|", tags)));
            }
            Recount();
        }
    }

    static class RDBEdUI
    {
        static RDBEdForm f;

        static void MakeCol<T>(string HeaderText, string DataPropertyName, float FillWeight) where T : DataGridViewCell, new()
        {
            if (Enum.GetName(typeof(EFieldIndices), f.gridMain.Columns.Count) != DataPropertyName) throw new Exception();
            DataGridViewColumn res = new DataGridViewColumn(new T());
            res.HeaderText = HeaderText;
            res.DataPropertyName = "Col" + DataPropertyName;
            res.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
            res.SortMode = DataGridViewColumnSortMode.Programmatic;
            if (FillWeight > 0) res.FillWeight = FillWeight;
            else { res.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; res.Resizable = DataGridViewTriState.False; res.Width = 20; }
            f.gridMain.Columns.Add(res);
        }

        public static void UpdateCounts(bool forceUpdateTitle = false)
        {
            int changes = (Data.Modifications + Data.Deletions);
            if (forceUpdateTitle || f.statusChanges.Text.Equals("0") != (changes == 0))
            {
                f.Text = f.OriginalText + " - " + Data.LoadedFileName + (changes != 0 ? "*" : "");
                f.menuSave.Enabled = f.menuExportModifications.Enabled = (changes != 0);
            }
            f.statusEntries.Text = Data.AllEntries.Count.ToString();
            f.statusFiltered.Text = Data.Filter.Count.ToString();
            f.statusChanges.LinkColor = (Data.Modifications == 0 ? SystemColors.ControlText : Entry.CellFieldColors[(int)EFieldFlag.Modified]);
            f.statusChanges.Text = Data.Modifications.ToString();
            f.statusWarnings.LinkColor = (Data.Warnings == 0 ? SystemColors.ControlText : Entry.CellFieldColors[(int)EFieldFlag.Warning]);
            f.statusWarnings.Text = Data.Warnings.ToString();
        }

        static bool Save(string saveFile, bool keepLoaded = false, bool forceRDB = false, bool forceDAT = false)
        {
            if (String.ReferenceEquals(saveFile, Data.DefaultNewFileName))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "RDB file (*.rdb)|*.rdb|DAT file (*.dat)|*.dat|All files (*.*)|*.*";
                sfd.DefaultExt = "rdb";
                saveFile = (sfd.ShowDialog() == DialogResult.OK ? sfd.FileName : null);
                sfd.Dispose();
            }
            if (saveFile == null) return false;
            try
            {
                if (forceRDB || (!forceDAT && saveFile.EndsWith(".rdb", StringComparison.InvariantCultureIgnoreCase)))
                    Data.AllEntries.SaveRDB(saveFile);
                else
                    Data.AllEntries.SaveDAT(saveFile);
                if (keepLoaded)
                {
                    Data.LoadedFileName = saveFile;
                    Data.ClearModificationFlags();
                    f.gridMain.Refresh();
                }
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return false; }
        }

        static bool CancelIfUnsaved()
        {
            int changes = (Data.Modifications + Data.Deletions);
            if (changes == 0) return false;
            DialogResult res = MessageBox.Show("Do you want to save changes to " + Data.LoadedFileName + "?", "Unsaved Changes", MessageBoxButtons.YesNoCancel);
            return ((res == DialogResult.Yes && !Save(Data.LoadedFileName)) || (res == DialogResult.Cancel));
        }

        static void RefreshBinding(bool updateCount)
        {
            Data.Filter.BroadcastListChanged();
            if (updateCount) RDBEdUI.UpdateCounts();
        }

        static void RefreshSort()
        {
            if (Data.SortFn != null) Data.Filter.Sort(Data.SortFn);
            else { Data.SetFilter(f.txtFilter.Text, f.statusChangesFiltered.Visible, f.statusWarningsFiltered.Visible); }
            RefreshBinding(updateCount: true);
        }

        static void RefreshFilter()
        {
            int fdRow    = (f.gridMain.FirstDisplayedCell != null ? Math.Max(f.gridMain.FirstDisplayedCell.RowIndex   , 0) : 0);
            int fdColumn = (f.gridMain.FirstDisplayedCell != null ? Math.Max(f.gridMain.FirstDisplayedCell.ColumnIndex, 0) : 0);
            Data.SetFilter(f.txtFilter.Text, f.statusChangesFiltered.Visible, f.statusWarningsFiltered.Visible);
            RefreshSort();
            if (fdRow < f.gridMain.RowCount) f.gridMain.FirstDisplayedCell = f.gridMain[fdColumn, fdRow];
        }

        static void OpenMultiForm(string title, string info, Action<MultiForm, string> onOK,
            string in1lbl = null, string in1filter = null, string in2lbl = null, string in2filter = null, string outlbl = null, string outfilter = null,
            string option1 = null, string option2 = null, bool countUniques = true, bool defaultNameKey = false)
        {
            MultiForm mf = new MultiForm();
            mf.Text = title;
            mf.lblInfo.Text = info;
            int decorationHeight = mf.Size.Height - mf.ClientSize.Height;
            Action onResize = () =>
            {
                mf.MinimumSize = new Size(mf.MinimumSize.Width, mf.tableLayout.Size.Height + decorationHeight);
                mf.MaximumSize = new Size(mf.MaximumSize.Width, mf.tableLayout.Size.Height + decorationHeight);
            };
            Action onChange = () => mf.btnOK.Enabled = 
                (!mf.in1Path.Visible || File.Exists(mf.in1Path.Text)) && 
                (!mf.in2Path.Visible || File.Exists(mf.in2Path.Text)) &&
                (!mf.outPath.Visible || !String.IsNullOrWhiteSpace(mf.outPath.Text));
            mf.Resize += (object s2, EventArgs e2) => onResize();
            mf.VisibleChanged += (object s, EventArgs e) => { onChange(); onResize(); };
            Action<bool, string, TextBox> onButton = (bool isIn, string filter, TextBox txt) =>
            {
                FileDialog fd = (isIn ? (FileDialog)new OpenFileDialog() : (FileDialog)new SaveFileDialog());
                fd.Filter = filter;
                fd.DefaultExt = "dat";
                fd.CheckFileExists = isIn;
                if (fd.ShowDialog() == DialogResult.OK) txt.Text = fd.FileName;
                fd.Dispose();
            };
            if (in1lbl == null)
            {
                mf.in1Label.Visible = mf.in1Path.Visible = mf.in1Button.Visible = false;
                mf.tableLayout.RowStyles[1].Height = 0;
            }
            else
            {
                mf.in1Label.Text = in1lbl;
                mf.in1Path.TextChanged += (object s, EventArgs e) => onChange();
                mf.in1Button.Click += (object s, EventArgs e) => onButton(true, in1filter, mf.in1Path);
            }
            if (in2lbl == null)
            {
                mf.in2Label.Visible = mf.in2Path.Visible = mf.in2Button.Visible = false;
                mf.tableLayout.RowStyles[2].Height = 0;
            }
            else
            {
                mf.in2Label.Text = in2lbl;
                mf.in2Path.TextChanged += (object s, EventArgs e) => onChange();
                mf.in2Button.Click += (object s, EventArgs e) => onButton(true, in2filter, mf.in2Path);
            }
            if (outlbl == null)
            {
                mf.outLabel.Visible = mf.outPath.Visible = mf.outButton.Visible = false;
                mf.tableLayout.RowStyles[3].Height = 0;
            }
            else
            {
                mf.outLabel.Text = outlbl;
                mf.outPath.TextChanged += (object s, EventArgs e) => onChange();
                mf.outButton.Click += (object s, EventArgs e) => onButton(false, outfilter, mf.outPath);
            }
            if (option1 == null)
            {
                mf.option1Check.Visible = false;
                mf.tableLayout.RowStyles[5].Height = 0;
            }
            else
            {
                mf.option1Check.Text = option1;
            }
            if (option2 == null)
            {
                mf.option2Check.Visible = false;
                mf.tableLayout.RowStyles[6].Height = 0;
            }
            else
            {
                mf.option2Check.Text = option2;
            }
            if (countUniques)
            {
                Dictionary<string, bool> dName = new Dictionary<string,bool>(), dRomName = new Dictionary<string,bool>(), dCRC = new Dictionary<string,bool>(), dMD5 = new Dictionary<string,bool>(), dSHA1 = new Dictionary<string,bool>(), dSerial = new Dictionary<string,bool>();
                foreach (Entry e in Data.AllEntries)
                {
                    if (!String.IsNullOrEmpty(e.Name   )) dName   [e.Name   ] = true;
                    if (!String.IsNullOrEmpty(e.RomName)) dRomName[e.RomName] = true;
                    if (!String.IsNullOrEmpty(e.CRC    )) dCRC    [e.CRC    ] = true;
                    if (!String.IsNullOrEmpty(e.MD5    )) dMD5    [e.MD5    ] = true;
                    if (!String.IsNullOrEmpty(e.SHA1   )) dSHA1   [e.SHA1   ] = true;
                    if (!String.IsNullOrEmpty(e.Serial )) dSerial [e.Serial ] = true;
                }
                mf.mergeKey.Items.Add("Name ("    + dName   .Count + " unique of " + Data.AllEntries.Count + ")");
                mf.mergeKey.Items.Add("RomName (" + dRomName.Count + " unique of " + Data.AllEntries.Count + ")");
                mf.mergeKey.Items.Add("CRC ("     + dCRC    .Count + " unique of " + Data.AllEntries.Count + ")");
                mf.mergeKey.Items.Add("MD5 ("     + dMD5    .Count + " unique of " + Data.AllEntries.Count + ")");
                mf.mergeKey.Items.Add("SHA1 ("    + dSHA1   .Count + " unique of " + Data.AllEntries.Count + ")");
                mf.mergeKey.Items.Add("Serial ("  + dSerial .Count + " unique of " + Data.AllEntries.Count + ")");
                mf.mergeKey.SelectedIndex = (defaultNameKey ? 0 : (dRomName.Count > dCRC.Count && dRomName.Count > dSerial.Count ? 1 : (dSerial.Count > dCRC.Count ? 5 : 2)));
            }
            else
            {
                mf.mergeKey.Items.Add("Name");
                mf.mergeKey.Items.Add("RomName");
                mf.mergeKey.Items.Add("CRC");
                mf.mergeKey.Items.Add("MD5");
                mf.mergeKey.Items.Add("SHA1");
                mf.mergeKey.Items.Add("Serial");
                mf.mergeKey.SelectedIndex = 2;
            }
            mf.btnOK.Click += (object s, EventArgs e) =>
            {
                onOK(mf, mf.mergeKey.SelectedItem.ToString().Split(' ')[0]);
                mf.Close();
                f.gridMain.Refresh();
            };
            mf.btnCancel.Click += (object s, EventArgs e) => mf.Close();
            mf.ShowDialog();
        }

        [STAThread] static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            f = new RDBEdForm();

            // This removes the webview on platforms that don't have webbrowser support (Mono without libgluezilla)
            try { f.webView.Visible = false; f.webView.Visible = true; }
            catch { f.webView.Parent.Controls.Remove(f.webView); f.webView = null; }

            f.gridMain.SetDoubleBuffering();
            f.gridMain.AutoGenerateColumns = false;
            f.gridMain.AutoSize = false;
            f.gridMain.DataSource = Data.Filter;

            MakeCol<DataGridViewTextBoxCell>("Name"         , "Name"          , 1.5f);
            MakeCol<DataGridViewTextBoxCell>("Description"  , "Description"   , 1.5f);
            MakeCol<DataGridViewTextBoxCell>("Genre"        , "Genre"         , 1);
            MakeCol<DataGridViewTextBoxCell>("RomName"      , "RomName"       , 1.5f);
            MakeCol<DataGridViewTextBoxCell>("Size"         , "Size"          , 1);
            MakeCol<DataGridViewTextBoxCell>("Users"        , "Users"         , .75f);
            MakeCol<DataGridViewTextBoxCell>("Release"      , "Release"       , 1);
            MakeCol<DataGridViewCheckBoxCell>("Rumble"      , "Rumble"        , .75f);
            MakeCol<DataGridViewCheckBoxCell>("Analog"      , "Analog"        , .75f);
            MakeCol<DataGridViewCheckBoxCell>("Coop"        , "Coop"          , .75f);
            MakeCol<DataGridViewTextBoxCell>("EnhancementHW", "EnhancementHW" , 1);
            MakeCol<DataGridViewTextBoxCell>("Franchise"    , "Franchise"     , 1);
            MakeCol<DataGridViewTextBoxCell>("OriginalTitle", "OriginalTitle" , 1);
            MakeCol<DataGridViewTextBoxCell>("Developer"    , "Developer"     , 1);
            MakeCol<DataGridViewTextBoxCell>("Publisher"    , "Publisher"     , 1);
            MakeCol<DataGridViewTextBoxCell>("Origin"       , "Origin"        , 1);
            MakeCol<DataGridViewTextBoxCell>("Region"       , "Region"        , 1);
            MakeCol<DataGridViewTextBoxCell>("Tags"         , "Tags"          , 1);
            MakeCol<DataGridViewTextBoxCell>("CRC"          , "CRC"           , 1);
            MakeCol<DataGridViewTextBoxCell>("MD5"          , "MD5"           , 1);
            MakeCol<DataGridViewTextBoxCell>("SHA1"         , "SHA1"          , 1);
            MakeCol<DataGridViewTextBoxCell>("Serial"       , "Serial"        , 1);
            f.gridMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            f.gridMain.CellToolTipTextNeeded += (object sender, DataGridViewCellToolTipTextNeededEventArgs e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= Data.Filter.Count) return;
                ERowFlag w = Data.Filter[e.RowIndex].RowFlags;
                if (w == ERowFlag.None || (Data.Filter[e.RowIndex].FieldFlags[e.ColumnIndex] & EFieldFlag.Warning) == 0) return;
                e.ToolTipText = "";
                if ((w & ERowFlag.WarningNotUnique     ) != 0) e.ToolTipText += (e.ToolTipText.Length > 0 ? "\n" : "") + "Not unique";
                if ((w & ERowFlag.WarningInvalid       ) != 0) e.ToolTipText += (e.ToolTipText.Length > 0 ? "\n" : "") + "Field validation failed";
                if ((w & ERowFlag.WarningToolUnifyError) != 0) e.ToolTipText += (e.ToolTipText.Length > 0 ? "\n" : "") + "Conflict during meta data unification";
                if ((w & ERowFlag.WarningToolMergeError) != 0) e.ToolTipText += (e.ToolTipText.Length > 0 ? "\n" : "") + "Conflict during file merge";
            };

            f.gridMain.ColumnHeaderMouseClick += (object sender, DataGridViewCellMouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point menuPos = f.gridMain.PointToClient(Cursor.Position);
                    ContextMenu context = new ContextMenu();
                    foreach (DataGridViewColumn col in f.gridMain.Columns)
                    {
                        MenuItem i = context.MenuItems.Add(col.HeaderText);
                        i.Checked = col.Visible;
                        i.Tag = col;
                        i.Click += (object objI, EventArgs ee) =>
                        {
                            (objI as MenuItem).Checked ^= true;
                            ((DataGridViewColumn)(objI as MenuItem).Tag).Visible ^= true;
                            (objI as MenuItem).GetContextMenu().Show(f.gridMain, menuPos);
                        };
                    }
                    context.Show(f.gridMain, menuPos);
                    return;
                }
                SortOrder order = f.gridMain.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
                order = (order == SortOrder.Ascending ? SortOrder.Descending : (order == SortOrder.Descending ? SortOrder.None : SortOrder.Ascending));
                foreach (DataGridViewColumn c in f.gridMain.Columns) c.HeaderCell.SortGlyphDirection = SortOrder.None;
                f.gridMain.CancelEdit();
                f.gridMain.CurrentCell = null;
                Data.SetSortFn((order == SortOrder.None ? null : f.gridMain.Columns[e.ColumnIndex].DataPropertyName), (order == SortOrder.Descending));
                RefreshSort();
            };

            f.gridMain.CellFormatting += (object sender, DataGridViewCellFormattingEventArgs e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= Data.Filter.Count) return;
                EFieldFlag flag = Data.Filter[e.RowIndex].FieldFlags[e.ColumnIndex];
                if (flag != EFieldFlag.None)
                    e.CellStyle.BackColor = Entry.CellFieldColors[(int)flag];
            };

            f.gridMain.SelectionChanged += (object sender, EventArgs e) =>
            {
                if (!f.gridMain.Focused || f.gridMain.SelectedCells.Count != 1) { f.gridMain.EndEdit(); return; }
                f.gridMain.BeginEdit(true);
            };

            f.menuEditCut.Click += (object sender, EventArgs e) =>
            {
                if (f.OnCopy() && f.OnDelete()) return;
                TextBox ed = (f.gridMain.EditingControl as TextBox);
                if (ed != null && ed.SelectionLength > 0) ed.Cut();
            };

            f.menuEditCopy.Click += (object sender, EventArgs e) =>
            {
                if (f.OnCopy()) return;
                TextBox ed = (f.gridMain.EditingControl as TextBox);
                if (ed != null && ed.SelectionLength > 0) ed.Copy();
            };

            f.menuEditPaste.Click += (object sender, EventArgs e) =>
            {
                if (f.OnPaste()) return;
                TextBox ed = (f.gridMain.EditingControl as TextBox);
                if (ed != null && ed.SelectionLength > 0) ed.Paste();
            };

            f.menuEditDelete.Click += (object sender, EventArgs e) =>
            {
                if (f.OnDelete()) return;
                TextBox ed = (f.gridMain.EditingControl as TextBox);
                if (ed != null && ed.SelectionLength > 0) ed.Text = ed.Text.Remove(ed.SelectionStart, ed.SelectionLength);
            };

            f.OnCopy = () =>
            {
                if (f.gridMain.EditingControl != null || f.txtFilter.Focused) return false;
                DataGridViewCell[] cells = new DataGridViewCell[f.gridMain.SelectedCells.Count];
                f.gridMain.SelectedCells.CopyTo(cells, 0);
                System.Array.Sort<DataGridViewCell>(cells, (DataGridViewCell a, DataGridViewCell b) => { int rd = a.RowIndex - b.RowIndex; return (rd < 0 ? -1 : (rd > 0 ? 1 : (a.ColumnIndex < b.ColumnIndex ? -1 : 1))); });
                System.Text.StringBuilder res = new System.Text.StringBuilder();
                int lastRow = -1;
                foreach (DataGridViewCell c in cells)
                {
                    if (!c.Visible) continue;
                    if (c.RowIndex > lastRow) { if (lastRow >= 0) res.Append('\n'); lastRow = c.RowIndex; }
                    else res.Append('\t');
                    res.Append(c.Value == null ? "" : c.Value.ToString());
                }
                try { Clipboard.SetDataObject(res.ToString(), true, 2, 100); } catch { }
                return true;
            };

            f.OnDelete = () =>
            {
                if (f.gridMain.EditingControl != null || f.txtFilter.Focused) return false;
                foreach (DataGridViewCell c in f.gridMain.SelectedCells)
                    if (c.Value != null && c.Value.ToString().Length > 0)
                        c.Value = null;
                return true;
            };

            f.OnPaste = () =>
            {
                if (f.gridMain.EditingControl != null || f.txtFilter.Focused) return false;
                DataGridViewCell[] cells = new DataGridViewCell[f.gridMain.SelectedCells.Count];
                f.gridMain.SelectedCells.CopyTo(cells, 0);
                System.Array.Sort<DataGridViewCell>(cells, (DataGridViewCell a, DataGridViewCell b) => { int rd = a.RowIndex - b.RowIndex; return (rd < 0 ? -1 : (rd > 0 ? 1 : (a.ColumnIndex < b.ColumnIndex ? -1 : 1))); });
                List<string[]> lines = new List<string[]>();
                foreach (string line in Clipboard.GetText().Split('\n')) lines.Add(line.Split('\t'));
                int lastRow = -1, row = -1, col = -1;
                foreach (DataGridViewCell c in cells)
                {
                    if (c.RowIndex > lastRow) { lastRow = c.RowIndex; row++; col = 0; }
                    string[] line = lines[row % lines.Count];
                    string val = line[col++ % line.Length];
                    if (!(c.Value == null ? "" : c.Value.ToString()).Equals(val))
                        c.Value = val;
                }
                return true;
            };

            f.OnCtrlUpDown = (bool e) =>
            {
                if (!f.gridMain.ContainsFocus || f.gridMain.CurrentCell == null) return false;
                int x = f.gridMain.CurrentCell.ColumnIndex, y = f.gridMain.CurrentCell.RowIndex;
                if (e)
                {
                    for (y++; y < Data.Filter.Count; y++)
                        if (f.gridMain[x, y].Value == null)
                            break;
                    if (y >= Data.Filter.Count) return false;
                }
                else
                {
                    for (y--; y >= 0; y--)
                        if (f.gridMain[x, y].Value == null)
                            break;
                    if (y < 0) return false;
                }
                f.gridMain.CurrentCell = f.gridMain[x, y];
                return true;
            };

            f.gridMain.DataError += (object sender, DataGridViewDataErrorEventArgs e) =>
            {
                e.Cancel = true;
                f.gridMain.CancelEdit();
                if (!f.gridMain.Rows[e.RowIndex].Displayed)
                    f.gridMain.FirstDisplayedScrollingRowIndex = e.RowIndex;
            };

            f.gridMain.Click += (object sender, EventArgs e) =>
            {
                MouseEventArgs me = e as MouseEventArgs;
                if (me == null || me.Button != MouseButtons.Right) return;
                if (f.gridMain.HitTest(me.X, me.Y).Type != DataGridViewHitTestType.None) return;
                ContextMenu context = new ContextMenu();
                context.MenuItems.Add("Add New Row").Click += (object objI, EventArgs ee) =>
                {
                    int fdRow    = (f.gridMain.FirstDisplayedCell != null ? f.gridMain.FirstDisplayedCell.RowIndex    : 0);
                    int fdColumn = (f.gridMain.FirstDisplayedCell != null ? f.gridMain.FirstDisplayedCell.ColumnIndex : 0);
                    Entry entry = new Entry();
                    Data.Filter.Add(entry);
                    Data.AllEntries.Add(entry);
                    RefreshBinding(updateCount: true);
                    f.gridMain.FirstDisplayedCell = f.gridMain[fdColumn, fdRow];
                };
                context.Show(f.gridMain, f.gridMain.PointToClient(Cursor.Position));
            };

            f.gridMain.CellMouseClick += (object sender, DataGridViewCellMouseEventArgs e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= Data.Filter.Count || e.ColumnIndex < 0 || e.Button != MouseButtons.Right) return;
                DataGridViewCell cell = f.gridMain[e.ColumnIndex, e.RowIndex];

                ContextMenu context = new ContextMenu();

                if (cell.Selected)
                {
                    int revertable = 0, rowFirst = int.MaxValue, rowLast = int.MinValue;
                    foreach (DataGridViewCell c in f.gridMain.SelectedCells)
                    {
                        Entry entry = Data.Filter[c.RowIndex];
                        if ((entry.FieldFlags[c.ColumnIndex] & EFieldFlag.Modified) != 0 && (entry.RowFlags & ERowFlag.FromFile) != 0) revertable++;
                        if (c.RowIndex < rowFirst) rowFirst = c.RowIndex;
                        if (c.RowIndex > rowLast ) rowLast  = c.RowIndex;
                    }

                    if (revertable != 0)
                    {
                        context.MenuItems.Add("Revert " + (revertable != 1 ? revertable + " Values" : "Value")).Click += (object objI, EventArgs ee) =>
                        {
                            if (MessageBox.Show("Are you sure you want to revert " + revertable + " value" + (revertable != 1 ? "s" : "") + "?", "Revert", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                            foreach (DataGridViewCell c in f.gridMain.SelectedCells)
                            {
                                Entry entry = Data.Filter[c.RowIndex];
                                if ((entry.FieldFlags[c.ColumnIndex] & EFieldFlag.Modified) == 0 || (entry.RowFlags & ERowFlag.FromFile) == 0) continue;
                                entry.Revert(f.gridMain.Columns[c.ColumnIndex].DataPropertyName);
                                f.gridMain.InvalidateCell(c);
                            }
                        };
                    }

                    int rowCount = (rowLast - rowFirst) + 1;
                    if (context.MenuItems.Count != 0) context.MenuItems.Add("-");
                    Action<int, int> AddRows = (int filterIdx, int allIdx) =>
                    {
                        int fdRow = f.gridMain.FirstDisplayedCell.RowIndex, fdColumn = f.gridMain.FirstDisplayedCell.ColumnIndex;
                        for (int i = 0; i != rowCount; i++)
                        {
                            Entry entry = new Entry();
                            Data.Filter.Insert(filterIdx, entry);
                            Data.AllEntries.Insert(allIdx, entry);
                        }
                        RefreshBinding(updateCount: true);
                        f.gridMain.FirstDisplayedCell = f.gridMain[fdColumn, fdRow];
                    };
                    string nRows = (rowCount > 1 ? " " + rowCount + " Rows" : " Row");
                    context.MenuItems.Add("Add" + nRows + " Above").Click += (object objI, EventArgs ee) => AddRows(rowFirst, Data.AllEntries.IndexOf(Data.Filter[rowFirst]));
                    context.MenuItems.Add("Add" + nRows + " Below").Click += (object objI, EventArgs ee) => AddRows(rowLast + 1, Data.AllEntries.IndexOf(Data.Filter[rowLast]) + 1);
                    context.MenuItems.Add("Remove" + nRows).Click += (object objI, EventArgs ee) =>
                    {
                        if (MessageBox.Show("Are you sure you want to remove " + rowCount + " row" + (rowCount > 1 ? "s" : "") + "?", "Remove", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                        int fdRow = f.gridMain.FirstDisplayedCell.RowIndex, fdColumn = f.gridMain.FirstDisplayedCell.ColumnIndex;
                        for (int i = rowLast; i >= rowFirst; i--)
                        {
                            Entry entry = Data.Filter[i];
                            if ((entry.RowFlags & ERowFlag.FromFile) != 0) Data.Deletions++;
                            Data.AllEntries.Remove(entry);
                            Data.Filter.RemoveAt(i);
                        }
                        if (Data.Filter.Count != 0)
                            f.gridMain.FirstDisplayedCell = f.gridMain[fdColumn, (fdRow >= Data.Filter.Count ? Data.Filter.Count - 1 : fdRow)];
                        Data.Recount();
                        RefreshBinding(updateCount: false);
                    };
                }
                else if ((Data.Filter[e.RowIndex].FieldFlags[e.ColumnIndex] & EFieldFlag.Modified) != 0 && (Data.Filter[e.RowIndex].RowFlags & ERowFlag.FromFile) != 0)
                {
                    context.MenuItems.Add("Revert").Click += (object objI, EventArgs ee) =>
                    {
                        Data.Filter[e.RowIndex].Revert(f.gridMain.Columns[e.ColumnIndex].DataPropertyName);
                    };
                }

                object cellValue = cell.Value;
                Action<bool> AppendFilter = (bool include) =>
                {
                    foreach (string s in (cellValue != null ? cellValue.ToString() : "").Split('|'))
                    {
                        bool quote = Regex.Match(s, @"[\""\|\*\s]").Success;
                        f.txtFilter.Text += (f.txtFilter.Text.Length == 0 ? "" : " ") + (include ? "" : "!") +
                            f.gridMain.Columns[e.ColumnIndex].DataPropertyName.Substring(3) + ":" +
                            (quote ? "\"|" + Regex.Replace(s, @"([\""\|])", "\\$1") + "|\"" : "|" + s + "|");
                    }
                };
                bool hasValue = (cellValue != null && cellValue.ToString().Length != 0);
                if (context.MenuItems.Count != 0) context.MenuItems.Add("-");
                if (hasValue)
                {
                    string cellStr = (cellValue.ToString().Length > 20 ? cellValue.ToString().Substring(0, 20) + "..." : cellValue.ToString());
                    context.MenuItems.Add("Filter '" + cellStr + "'").Click += (object objI, EventArgs ee) => { AppendFilter(true); };
                    context.MenuItems.Add("Filter NOT '" + cellStr + "'").Click += (object objI, EventArgs ee) => { AppendFilter(false); };
                }
                else
                {
                    context.MenuItems.Add("Filter Empty").Click += (object objI, EventArgs ee) => { AppendFilter(true); };
                    context.MenuItems.Add("Filter NOT Empty").Click += (object objI, EventArgs ee) => { AppendFilter(false); };
                }

                if (hasValue && f.webView != null)
                {
                    if (context.MenuItems.Count != 0) context.MenuItems.Add("-");
                    foreach (var u in new KeyValuePair<string, string>[]
                    {
                        new KeyValuePair<string, string>("Google",             "https://www.google.com/search?ie=utf-8&oe=utf-8&q=" ),
                        new KeyValuePair<string, string>("Wikipedia English",  "https://en.wikipedia.org/w/index.php?cirrusUserTesting=control&search="),
                        new KeyValuePair<string, string>("Wikipedia Japanese", "https://ja.wikipedia.org/w/index.php?cirrusUserTesting=control&search="),
                        new KeyValuePair<string, string>("GameFAQs",           "https://gamefaqs.gamespot.com/search?game="),
                        new KeyValuePair<string, string>("MobyGames",          "https://www.mobygames.com/search/quick?q="),
                        new KeyValuePair<string, string>("GamesDatabase",      "https://www.gamesdatabase.org/list.aspx?in=1&searchtype=1&searchtext="),
                        new KeyValuePair<string, string>("TGDB",               "https://thegamesdb.net/search.php?name="),
                        new KeyValuePair<string, string>("IGDB",               "https://www.igdb.com/search?type=1&q=asdf"),
                    })
                    {
                        MenuItem i = context.MenuItems.Add(u.Key);
                        i.Click += (object objI, EventArgs ee) =>
                        {
                            if (f.splitContainer.Panel2Collapsed)
                            {
                                f.splitContainer.Panel2Collapsed = false;
                                f.splitContainer.SplitterDistance = f.splitContainer.Size.Height / 2;
                            }
                            f.webView.NavigateModernBrowser(u.Value + f.gridMain[e.ColumnIndex, e.RowIndex].Value.ToString());
                        };
                    }
                }
                if (context.MenuItems.Count != 0)
                    context.Show(f.gridMain, f.gridMain.PointToClient(Cursor.Position));
                return;
            };

            f.splitContainer.SplitterMoved += (object sender, SplitterEventArgs e) =>
            {
                if (f.splitContainer.Panel2.Size.Height <= f.splitContainer.Panel2MinSize + 5)
                    f.splitContainer.Panel2Collapsed = true;
            };

            f.txtFilter.TextChanged += (object sender, EventArgs e) =>
            {
                RefreshFilter();
            };

            f.btnClearFilter.Click += (object sender, EventArgs e) =>
            {
                f.statusChangesFiltered.Visible = false;
                f.statusWarningsFiltered.Visible = false;
                if (string.IsNullOrEmpty(f.txtFilter.Text)) RefreshFilter();
                else f.txtFilter.Text = "";
            };

            f.statusChanges.Click += (object sender, EventArgs e) =>
            {
                f.statusChangesFiltered.Visible ^= true;
                RefreshFilter();
            };

            f.statusWarnings.Click += (object sender, EventArgs e) =>
            {
                f.statusWarningsFiltered.Visible ^= true;
                RefreshFilter();
            };

            f.menuNew.Click += (object sender, EventArgs e) =>
            {
                if (CancelIfUnsaved()) return;
                Data.Init(null);
                RefreshFilter();
            };

            f.menuOpen.Click += (object sender, EventArgs e) =>
            {
                if (CancelIfUnsaved()) return;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.CheckFileExists = true;
                ofd.Filter = "RDB or DAT file|*.rdb;*.dat|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Data.Init(ofd.FileName);
                    RefreshFilter();
                }
                ofd.Dispose();
            };

            f.menuImport.Click += (object sender, EventArgs e) =>
            {
                OpenMultiForm("Import File",
                    "This imports all fields from an additional file according to the merge key specified",
                    in1lbl: "File:", in1filter: "RDB or DAT file|*.rdb;*.dat|All files (*.*)|*.*",
                    option1: "Add entries without match as new", option2: "Mark fields with conflicts during import (and keep both values)",
                    onOK: (MultiForm mf, string key) =>
                {
                    Data.Import(mf.in1Path.Text, key, mf.option1Check.Checked, mf.option2Check.Checked);
                    RefreshFilter();
                });
            };

            f.menuSave.Click += (object sender, EventArgs e) =>
            {
                if (Data.Modifications == 0 && Data.Deletions == 0) return;
                Save(Data.LoadedFileName, true);
            };

            f.menuSaveAs.Click += (object sender, EventArgs e) =>
            {
                Save(Data.DefaultNewFileName, true);
            };

            f.menuExportModifications.Click += (object sender, EventArgs e) =>
            {
                if (Data.Modifications == 0) return;
                OpenMultiForm("Export Modifications DAT",
                    "This will save all modifications done in this session to a DAT file.",
                    outfilter: "DAT file (*.dat)|*.dat|All files (*.*)|*.*", outlbl: "Output DAT:",
                    onOK: (MultiForm mf, string key) =>
                {
                    try { Data.AllEntries.SaveDAT(mf.outPath.Text, true, key); }
                    catch (Exception ex) { MessageBox.Show(ex.ToString(), "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                });
            };

            f.menuExportRDB.Click += (object sender, EventArgs e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "RDB file (*.rdb)|*.rdb|All files (*.*)|*.*";
                sfd.DefaultExt = "rdb";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try { Data.AllEntries.SaveRDB(sfd.FileName); }
                    catch (Exception ex) { MessageBox.Show(ex.ToString(), "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                }
                sfd.Dispose();
            };

            f.menuExportDAT.Click += (object sender, EventArgs e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "DAT file (*.dat)|*.dat|All files (*.*)|*.*";
                sfd.DefaultExt = "dat";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try { Data.AllEntries.SaveDAT(sfd.FileName); }
                    catch (Exception ex) { MessageBox.Show(ex.ToString(), "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                }
                sfd.Dispose();
            };

            f.menuToolUnify.Click += (object sender, EventArgs e) =>
            {
                OpenMultiForm("Unify Meta Data With Equal Field",
                    "This will unify all meta data fields of entries that share the exact same value of the key field.\n\n" +
                    "Fields merged: Genre, Users, Release, Rumble, Analog, Coop, EnhancementHW, Franchise, OriginalTitle, Developer, Publisher, Origin, Region, Tags\n\n" +
                    "Rows with conflicts during the process will be marked with a warning flag so they can be checked afterwards.",
                    defaultNameKey: true, onOK: (MultiForm mf, string key) =>
                { 
                    Data.UnifyMeta(key);
                    RefreshFilter();
                });
            };

            f.menuToolGenerateTags.Click += (object sender, EventArgs e) =>
            {
                Data.ToolGenerateTags();
                RefreshFilter();
            };

            f.menuToolDeltaDAT.Click += (object sender, EventArgs e) =>
            {
                OpenMultiForm("Create Delta DAT",
                    "This tool will create a new DAT file with all changes between the two selected input files.",
                    in1lbl: "Input File 1:", in1filter: "RDB or DAT file|*.rdb;*.dat|All files (*.*)|*.*",
                    in2lbl: "Input File 2:", in2filter: "RDB or DAT file|*.rdb;*.dat|All files (*.*)|*.*",
                    outlbl: "Output File:", outfilter: "DAT file (*.dat)|*.dat|All files (*.*)|*.*",
                    option1: "Add entries without match as new", countUniques: false, onOK: (MultiForm mf, string key) =>
                {
                    Data.GenerateDeltaDAT(mf.in1Path.Text, mf.in2Path.Text, mf.outPath.Text, key, mf.option2Check.Checked);
                });
            };

            f.menuValidateUnique.Click += (object sender, EventArgs e) =>
            {
                OpenMultiForm("Validate Unique Field",
                    "Select the field which you want to validate as being unique.",
                    onOK: (MultiForm mf, string key) =>
                {
                    Data.ValidateUnique(key);
                });
            };

            f.menuValidateField.Click += (object sender, EventArgs e) =>
            {
                RegexForm rf = new RegexForm();
                rf.cmbField.Items.Add("Name");
                rf.cmbField.Items.Add("Description");
                rf.cmbField.Items.Add("Genre");
                rf.cmbField.Items.Add("RomName");
                rf.cmbField.Items.Add("EnhancementHW");
                rf.cmbField.Items.Add("Franchise");
                rf.cmbField.Items.Add("Developer");
                rf.cmbField.Items.Add("Publisher");
                rf.cmbField.Items.Add("Origin");
                rf.cmbField.Items.Add("Region");
                rf.cmbField.Items.Add("Tags");
                rf.cmbField.Items.Add("Serial");
                rf.cmbField.SelectedItem = "Serial";
                rf.cmbPreset.Items.Add("Custom");
                rf.cmbPreset.Items.Add("SNES Serial");
                rf.cmbPreset.SelectedItem = "Custom";
                rf.cmbPreset.SelectedIndexChanged += (object s2, EventArgs e2) =>
                {
                    if (rf.cmbPreset.SelectedIndex == 1) // SNES Serial
                    {
                        rf.cmbField.SelectedItem = "Serial";
                        rf.txtRegex.Text =
                             @"^(                                                                                     | #allow empty"+"\r\n"
                            +@"SHVC-[ABZ][A-Z0-9]{2}J-JPN                                                             | #SHVC-AAGJ-JPN / SHVC-ZBSJ-JPN"+"\r\n"
                            +@"SHVC-[A-Z0-9]{2}                                                                       | #SHVC-T2"+"\r\n"
                            +@"SNS-(A[A-Z0-9]{2}[EB]|[A-Z0-9]{2})-(USA|LTN|CAN|BRA)                                   | #SNS-AWVE-USA / SNS-9D-LTN"+"\r\n"
                            +@"SNSP-(A[A-Z0-9]{2}[PFD]|[A-Z0-9]{2})-(UKV|SCN|NOE|ITA|HOL|GPS|FRG|FRA|FAH|EUR|ESP|AUS) | #SNSP-ADCP-NOE / SNSP-6F-UKV"+"\r\n"
                            +@"SNSN-(A[A-Z0-9]{2}[K]|[A-Z0-9]{2})-(ROC|HKG|KOR)                                       | #SNSN-S3-ROC / SNSN-AZ4K-KOR"+"\r\n"
                            +@"SHVC-TOBJ-JPN                                                                          | #Zaitaku Touhyou System - SPAT4-Wide"+"\r\n"
                            +@"SHVC-TJ[ABD]J-JPN                                                                      | #JRA PAT (full serial has suffix -[US][123])"+"\r\n"
                            +@"SNSP-A-SG-NOE|SNS-A-SG-USA|SHVC-SGB|SHVC-SGB2-JPN                                      | #Super GameBoy / Super GameBoy 2"+"\r\n"
                            +@"SHVC-MMSA-JPN                                                                          | #SFC Memory Cartridge"+"\r\n"
                            +@"CPC-RAMC-4M                                                                            | #GAME PROCESSOR RAM CASETTE (early version of memory cartridge?)"+"\r\n"
                            +@"CEJ01XB                                                                                  #XBAND (JPN)"+"\r\n"
                            +@")$";
                        rf.chkNegate.Checked = false;
                    }
                };
                rf.txtRegex.TextChanged += (object s2, EventArgs e2) =>
                {
                    rf.btnOK.Enabled = !string.IsNullOrWhiteSpace(rf.txtRegex.Text);
                };
                rf.btnOK.Enabled = false;
                rf.btnOK.Click += (object s2, EventArgs e2) =>
                {
                    if (!Data.ValidateRegex(rf.cmbField.SelectedItem.ToString(), rf.txtRegex.Text, rf.chkNegate.Checked))
                        return;
                    rf.Close();
                    f.gridMain.Refresh();
                };
                rf.btnCancel.Click += (object s2, EventArgs e2) => rf.Close();
                rf.ShowDialog();
            };

            f.menuValidateClear.Click += (object sender, EventArgs e) =>
            {
                foreach (Entry entry in Data.AllEntries)
                    if ((entry.RowFlags & ERowFlag.WarningsMask) != 0)
                        entry.ClearWarnings();
                f.gridMain.Refresh();
            };

            f.menuAbout.Click += (object sender, EventArgs e) =>
            {
                MessageBox.Show(About.Text, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            f.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                e.Cancel = CancelIfUnsaved();
            };

            f.menuExit.Click += (object sender, EventArgs e) =>
            {
                f.Close();
            };

            f.AllowDrop = true;
            f.DragEnter += (object sender, DragEventArgs e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
            };
            f.DragDrop += (object sender, DragEventArgs e) =>
            {
                string[] files = (e.Data.GetData(DataFormats.FileDrop) as string[]);
                if (files == null || files.Length < 1 || CancelIfUnsaved()) return;
                Data.Init(files[0]);
                RefreshFilter();
            };

            Data.Init(args.Length >= 1 ? args[0] : null);
            RefreshFilter();

            Application.Run(f);
            f.webView.CleanupModernBrowser();
        }
    }

    static class ExtensionMethods
    {
        //Enable double buffering on the data grid for fast rendering and scrolling (disable while resizing columns)
        public static void SetDoubleBuffering(this Control c)
        {
            c.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(c, true, null);
            if (!(c is DataGridView)) return;
            c.MouseDown += (object s, MouseEventArgs e) => { if (e.Y <= (s as DataGridView).ColumnHeadersHeight) s.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(s, false, null); };
            c.MouseUp   += (object s, MouseEventArgs e) => {                                                     s.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(s,  true, null); };
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        public static short  FromBigEndian(this short value)  { return System.Net.IPAddress.NetworkToHostOrder(value); }
        public static int    FromBigEndian(this int value)    { return System.Net.IPAddress.NetworkToHostOrder(value); }
        public static long   FromBigEndian(this long value)   { return System.Net.IPAddress.NetworkToHostOrder(value); }
        public static ushort FromBigEndian(this ushort value) { return unchecked((ushort)System.Net.IPAddress.NetworkToHostOrder(unchecked((short)value))); }
        public static uint   FromBigEndian(this uint value)   { return unchecked((uint  )System.Net.IPAddress.NetworkToHostOrder(unchecked((int  )value))); }
        public static ulong  FromBigEndian(this ulong value)  { return unchecked((ulong )System.Net.IPAddress.NetworkToHostOrder(unchecked((long )value))); }
        public static short  ToBigEndian(this short value)    { return System.Net.IPAddress.HostToNetworkOrder(value); }
        public static int    ToBigEndian(this int value)      { return System.Net.IPAddress.HostToNetworkOrder(value); }
        public static long   ToBigEndian(this long value)     { return System.Net.IPAddress.HostToNetworkOrder(value); }
        public static ushort ToBigEndian(this ushort value)   { return unchecked((ushort)System.Net.IPAddress.HostToNetworkOrder(unchecked((short)value))); }
        public static uint   ToBigEndian(this uint value)     { return unchecked((uint  )System.Net.IPAddress.HostToNetworkOrder(unchecked((int  )value))); }
        public static ulong  ToBigEndian(this ulong value)    { return unchecked((ulong )System.Net.IPAddress.HostToNetworkOrder(unchecked((long )value))); }

        [System.Runtime.InteropServices.DllImport("wininet.dll", SetLastError = true)]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        static void ClearBrowserSession()
        {
            try
            {
                // clear cache or become separate session than IExplore at least
                const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
                InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            }
            catch { }
        }

        static bool BrowserInited = false;
        public static void NavigateModernBrowser(this WebBrowser self, string urlString)
        {
            if (!BrowserInited)
            {
                BrowserInited = true;
                try
                {
                    const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";
                    const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);
                    string version = (key.GetValue("svcVersion", null) ?? key.GetValue("Version", null)).ToString();
                    int separator = version.IndexOf('.');
                    int ieVersion = int.Parse(version.Substring(0, separator));
                    int emulationCode = 0;
                    if      (ieVersion >= 11) emulationCode = 11001; //Version11Edge (or Version11 = 11000)
                    else if (ieVersion >= 10) emulationCode = 10000; //Version10 (or Version10Standards = 10001)
                    else if (ieVersion >=  9) emulationCode =  9000; //Version9 (or Version9Standards = 9999)
                    else if (ieVersion >=  8) emulationCode =  8000; //Version8 (or Version8Standards = 8888)
                    else                      emulationCode =  7000; //Version7

                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
                    string programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                    object value = key.GetValue(programName, null);
                    if (value != null && Convert.ToInt32(value) == emulationCode) return;

                    if (emulationCode != 0)
                        key.SetValue(programName, emulationCode, Microsoft.Win32.RegistryValueKind.DWord);
                    else
                        key.DeleteValue(programName, false);
                }
                catch { }
            }
            ClearBrowserSession();
            self.Navigate(urlString);
        }

        public static void CleanupModernBrowser(this WebBrowser self)
        {
            if (!BrowserInited) return;
            BrowserInited = false;
            ClearBrowserSession();
            try
            {
                const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";
                const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
                string programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                key.DeleteValue(programName, false);
            }
            catch { }
        }
    }
}
