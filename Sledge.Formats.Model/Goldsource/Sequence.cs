﻿using System.Numerics;

namespace Sledge.Formats.Model.Goldsource
{
    public class Sequence
    {
        public string Name;

        public float Framerate;
        public int Flags;

        public int Activity;
        public int ActivityWeight;

        public int NumEvents;
        public int EventIndex;

        public int NumFrames;

        public int NumPivots;
        public int PivotIndex;

        public int MotionType;
        public int MotionBone;
        public Vector3 LinearMovement;
        public int AutoMovePositionIndex; // unused
        public int AutoMoveAngleIndex; // unused

        public Vector3 Min;
        public Vector3 Max;

        public int NumBlends;
        public int AnimationIndex;
        public int[] BlendType;
        public float[] BlendStart;
        public float[] BlendEnd;
        public int BlendParent;

        public int SequenceGroup;

        public int EntryNode;
        public int ExitNode;
        public int NodeFlags;

        public int NextSequence;

        public Blend[] Blends;
        public AnimationEvent[] Events { get; set; }
        public Pivot[] Pivots { get; set; }
    }
}