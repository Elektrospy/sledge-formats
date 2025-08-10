﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sledge.Formats.Bsp.Objects;

namespace Sledge.Formats.Bsp.Lumps
{
    public class Entities : ILump, IList<Entity>
    {
        private readonly IList<Entity> _entities;

        public Entities()
        {
            _entities = new List<Entity>();
        }

        public void Read(BinaryReader br, Blob blob, Version version)
        {
            var text = Encoding.ASCII.GetString(br.ReadBytes(blob.Length));

            Entity cur = null;
            int i;
            string key = null;
            for (i = 0; i < text.Length; i++)
            {
                var token = GetToken();
                if (token == "{")
                {
                    // Start of new entity
                    cur = new Entity();
                    _entities.Add(cur);
                    key = null;
                }
                else if (token == "}")
                {
                    // End of entity
                    cur = null;
                    key = null;
                }
                else if (cur != null && key != null)
                {
                    // KeyValue value
                    SetKeyValue(cur, key, token);
                    key = null;
                }
                else if (cur != null)
                {
                    // KeyValue key
                    key = token;
                }
                else if (token == null)
                {
                    // End of file
                    break;
                }
                else
                {
                    // Invalid
                }
            }

            string GetToken()
            {
                if (!ScanToNonWhitespace()) return null;

                if (text[i] == '/' && i + 1 < text.Length && text[i + 1] == '/')
                {
                    // Comment, find end of line and then skip whitespace again
                    var idx = text.IndexOf('\n', i + 1);
                    if (idx < 0) return null;
                    i = idx + 1;
                    if (!ScanToNonWhitespace()) return null;
                }

                if (text[i] == '{' || text[i] == '}')
                {
                    // Start/end entity
                    return text[i].ToString();
                }

                if (text[i] == '"')
                {
                    // Quoted string, find end quote
                    var idx = text.IndexOf('"', i + 1);
                    if (idx < 0) return null;
                    var tok = text.Substring(i + 1, idx - i - 1);
                    i = idx + 1;
                    return tok;
                }

                if (text[i] > 32)
                {
                    // Not whitespace
                    var s = "";
                    while (text[i] > 32)
                    {
                        s += text[i++];
                    }
                    return s;
                }

                return null;
            }

            bool ScanToNonWhitespace()
            {
                while (i < text.Length)
                {
                    if (text[i] == ' ' || text[i] == '\n') i++;
                    else return true;
                }

                return false;
            }

            void SetKeyValue(Entity e, string k, string v)
            {
                e.Set(k, v);
            }
        }

        public void PostReadProcess(BspFile bsp)
        {
            
        }

        public void PreWriteProcess(BspFile bsp, Version version)
        {
            
        }

        public int Write(BinaryWriter bw, Version version)
        {
            var pos = bw.BaseStream.Position;
            var sb = new StringBuilder();
            foreach (var entity in _entities)
            {
                sb.Append("{\n");

                foreach (var kv in entity.SortedKeyValues.Where(x => x.Key?.Length > 0 && x.Value?.Length > 0))
                {
                    sb.Append($"\"{kv.Key}\" \"{kv.Value}\"\n");
                }

                sb.Append("}\n");
            }
            bw.Write(Encoding.ASCII.GetBytes(sb.ToString()));
            //Null terminate string.
            bw.Write((byte)0);
            return (int)(bw.BaseStream.Position - pos);
        }

        #region IList

        public IEnumerator<Entity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _entities).GetEnumerator();
        }

        public void Add(Entity item)
        {
            _entities.Add(item);
        }

        public void Clear()
        {
            _entities.Clear();
        }

        public bool Contains(Entity item)
        {
            return _entities.Contains(item);
        }

        public void CopyTo(Entity[] array, int arrayIndex)
        {
            _entities.CopyTo(array, arrayIndex);
        }

        public bool Remove(Entity item)
        {
            return _entities.Remove(item);
        }

        public int Count => _entities.Count;

        public bool IsReadOnly => _entities.IsReadOnly;

        public int IndexOf(Entity item)
        {
            return _entities.IndexOf(item);
        }

        public void Insert(int index, Entity item)
        {
            _entities.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _entities.RemoveAt(index);
        }

        public Entity this[int index]
        {
            get => _entities[index];
            set => _entities[index] = value;
        }

        #endregion
    }
}