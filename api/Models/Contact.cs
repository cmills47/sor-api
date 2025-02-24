// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace SiteOfRefuge.API.Models
{
    /// <summary> Contact information of a person. </summary>
    public partial class Contact
    {
        public Contact() {}

        /// <summary> Initializes a new instance of Contact. </summary>
        /// <param name="name"> The person&apos;s full name. </param>
        /// <param name="methods"> The way(s) in which this person can be contacted. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> or <paramref name="methods"/> is null. </exception>
        public Contact(string name, List<ContactMode> methods)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (methods == null)
            {
                throw new ArgumentNullException(nameof(methods));
            }

            Name = name;
            Methods = methods;
        }

        /// <summary> Initializes a new instance of Contact. </summary>
        /// <param name="id"> Unique identifier in UUID/GUID format. </param>
        /// <param name="name"> The person&apos;s full name. </param>
        /// <param name="methods"> The way(s) in which this person can be contacted. </param>
        internal Contact(Guid? id, string name, IList<ContactMode> methods)
        {
            Id = id;
            Name = name;
            Methods = methods;
        }

        /// <summary> Unique identifier in UUID/GUID format. </summary>
        public Guid? Id { get; set; }
        /// <summary> The person&apos;s full name. </summary>
        public string Name { get; set; }
        /// <summary> The way(s) in which this person can be contacted. </summary>
        public IList<ContactMode> Methods { get; }
    }
}
