// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using SiteOfRefuge.API;

namespace SiteOfRefuge.API.Models
{
    /// <summary> The Availability. </summary>
    public partial class Availability
    {
        /// <summary> Initializes a new instance of Availability. </summary>
        public Availability()
        {
        }

        /// <summary> Initializes a new instance of Availability. </summary>
        /// <param name="id"> Unique identifier in UUID/GUID format. </param>
        /// <param name="dateAvailable"> Date when shelter will be available. </param>
        /// <param name="active"> Is this shelter currently available for matching with refugees?. </param>
        /// <param name="lengthOfStay"> How long a refugee can stay. </param>
        internal Availability(Guid? id, DateTimeOffset? dateAvailable, bool? active, AvailabilityLengthOfStay? lengthOfStay)
        {
            Id = id;
            DateAvailable = dateAvailable;
            Active = active;
            LengthOfStay = lengthOfStay;
        }

        /// <summary> Unique identifier in UUID/GUID format. </summary>
        public Guid? Id { get; set; }
        /// <summary> Date when shelter will be available. </summary>
        public DateTimeOffset? DateAvailable { get; set; }
        /// <summary> Is this shelter currently available for matching with refugees?. </summary>
        public bool? Active { get; set; }
        /// <summary> How long a refugee can stay. </summary>
        public AvailabilityLengthOfStay? LengthOfStay { get; set; }
    }
}
