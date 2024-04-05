﻿using Colossal.Entities;
using ExtendedTooltip.Systems;
using Game.Buildings;
using Game.Prefabs;
using Game.UI.Tooltip;
using Unity.Entities;

namespace ExtendedTooltip.TooltipBuilder
{
    public class EducationTooltipBuilder : TooltipBuilderBase
    {
        public EducationTooltipBuilder(EntityManager entityManager, CustomTranslationSystem customTranslationSystem)
        : base(entityManager, customTranslationSystem)
        {
            Mod.DebugLog($"Created EducationTooltipBuilder.");
        }

        public void Build(Entity selectedEntity, Entity prefab, TooltipGroup tooltipGroup)
        {
            if (m_ExtendedTooltipSystem.Model.ShowEducationStudentCapacity == false)
                return;

            // Add student info if available
            int studentCount = 0;
            int studentCapacity = 0;
            if (UpgradeUtils.TryGetCombinedComponent(m_EntityManager, selectedEntity, prefab, out SchoolData schoolData))
            {
                studentCapacity = schoolData.m_StudentCapacity;
            }
            if (m_EntityManager.TryGetBuffer(selectedEntity, true, out DynamicBuffer<Student> studentsBuffer))
            {
                studentCount = studentsBuffer.Length;
            }

            if (studentCapacity > 0)
            {
                StringTooltip studentTooltip = new()
                {
                    icon = "Media/Game/Icons/Population.svg",
                    value = m_CustomTranslationSystem.GetLocalGameTranslation("SelectedInfoPanel.EDUCATION_STUDENTS", "Students") + ": " + studentCount + "/" + studentCapacity,
                    color = (studentCount == 0) ? TooltipColor.Info : (studentCount * 100 / studentCapacity) <= 50 ? TooltipColor.Success : (studentCount * 100 / studentCapacity) <= 90 ? TooltipColor.Warning : TooltipColor.Error,
                };

                tooltipGroup.children.Add(studentTooltip);
            }
        }
    }
}
