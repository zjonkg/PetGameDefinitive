using UnityEngine;

public static class InputState
{
    public static bool IsDragging = false;
    public static float LastDragEndTime = -10f;

    public static bool IsSwipeBlocked => IsDragging || Time.time - LastDragEndTime < 0.2f;
}
