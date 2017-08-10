﻿using System;
using System.Collections.Generic;

namespace Zametek.Common.Project
{
    [Serializable]
    public class ProjectPlanDto
    {
        //public string Title { get; set; }
        public DateTime ProjectStart { get; set; }
        public List<ResourceDto> Resources { get; set; }
        public bool DisableResources { get; set; }
        public ArrowGraphSettingsDto ArrowGraphSettings { get; set; }

        public bool AllResourcesExplicitTargetsButNotAllActivitiesTargeted { get; set; }
        public List<CircularDependencyDto> CircularDependencies { get; set; }
        public List<int> MissingDependencies { get; set; }
        public List<DependentActivityDto> DependentActivities { get; set; }
        public List<ResourceScheduleDto> ResourceSchedules { get; set; }

        public ArrowGraphDto ArrowGraph { get; set; }
        public bool HasStaleArrowGraph { get; set; }

        public bool HasStaleOutputs { get; set; }
        //public VertexGraphDto VertexGraph { get; set; }
    }
}
