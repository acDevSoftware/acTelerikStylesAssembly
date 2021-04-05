/*
 * Copyright (c) 2012 acDevSoftware
 * http://www.acdevsoftware.ch
 * 
 * This file is part of acTelerikStylesAssembly.
 *
 * acTelerikStylesAssembly is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * acTelerikStylesAssembly is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.

 * You should have received a copy of the GNU Lesser General Public License
 * along with acTelerikStylesAssembly.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace acTelerikStylesAssembly
{
    /* Copyright 2012 Telerik, modified by acDevSoftware */
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class EmbeddedSkinAttribute : Attribute
    {
        private string m_sShortControlName;
        private string m_sSkin;
        private Type m_nType;

        public EmbeddedSkinAttribute(string shortControlName)
        {
            m_sShortControlName = shortControlName;
        }
        public EmbeddedSkinAttribute(string shortControlName, string skin)
        {
            m_sShortControlName = shortControlName;
            m_sSkin = skin;
        }
        public EmbeddedSkinAttribute(string shortControlName, Type type)
        {
            m_sShortControlName = shortControlName;
            m_nType = type;
        }
        public EmbeddedSkinAttribute(string shortControlName, string skin, Type type)
        {
            m_sShortControlName = shortControlName;
            m_sSkin = skin;
            m_nType = type;
        }

        public string CssResourceName { get { return string.Empty; } }
        public bool IsCommonCss { get { return false; } }
        public string ShortControlName { get { return m_sShortControlName; } }
        public string Skin { get { return m_sSkin; } }
        public Type Type { get { return m_nType; } }
    }
}
