using UnityEngine;


public class Bounds2 {
    public Vector2 extend;
    public Vector2 center;

    public Vector2 Size { get { return extend * 2; } }

    public Vector2 BottomLeft { get {return center-extend; } }
    public Vector2 BottomRight { get {return new Vector2(center.x + extend.x, center.y - extend.y); } }
    public Vector2 TopLeft { get {return new Vector2(center.x - extend.x, center.y + extend.y); }  }
    public Vector2 TopRight { get {return center + extend; } }

    public Vector2 Top { get {return center + extend * Vector2.up; } }
    public Vector2 Bottom { get {return center + extend * Vector2.down; } }
    public Vector2 Left { get {return center + extend * Vector2.left; } }
    public Vector2 Right { get {return center + extend * Vector2.right; } }
    
    public Bounds2(Vector2 min, Vector2 max) {
        this.extend = (max-min) / 2;
        this.center = min + this.extend;
    }
}

public class UIBounds {
    public Bounds2 ScreenPointBounds;
    public Bounds2 ViewportBounds;

    private UIBounds(Bounds2 ScreenPointBounds, Bounds2 ViewportBounds) {
        this.ScreenPointBounds = ScreenPointBounds;
        this.ViewportBounds = ViewportBounds;
    }

    public static UIBounds fromBounds(Bounds bounds) {
        return fromBounds(bounds, Camera.main);
    }

    public static UIBounds fromBounds(Bounds bounds, Camera camera) {
        Vector2 screenPointMin = Vector2.positiveInfinity;
        Vector2 screenPointMax = Vector2.negativeInfinity;

        Vector2 viewportMin = Vector2.positiveInfinity;
        Vector2 viewportMax = Vector2.negativeInfinity;

        for(int i=0; i<8; i++) {
            Vector3 corner = new Vector3(
                (i & (1<<0)) != 0 ? bounds.min.x : bounds.max.x,
                (i & (1<<1)) != 0 ? bounds.min.y : bounds.max.y,
                (i & (1<<2)) != 0 ? bounds.min.z : bounds.max.z
            );

            Vector3 screenPointCorner = Camera.main.WorldToScreenPoint(corner);
            screenPointMin.x = Mathf.Min(screenPointMin.x, screenPointCorner.x);
            screenPointMin.y = Mathf.Min(screenPointMin.y, screenPointCorner.y);
            screenPointMax.x = Mathf.Max(screenPointMax.x, screenPointCorner.x);
            screenPointMax.y = Mathf.Max(screenPointMax.y, screenPointCorner.y);

            Vector3 viewportCorner = Camera.main.WorldToViewportPoint(corner);
            viewportMin.x = Mathf.Min(viewportMin.x, viewportCorner.x);
            viewportMin.y = Mathf.Min(viewportMin.y, viewportCorner.y);
            viewportMax.x = Mathf.Max(viewportMax.x, viewportCorner.x);
            viewportMax.y = Mathf.Max(viewportMax.y, viewportCorner.y);

        }
        return new UIBounds(
            new Bounds2(screenPointMin, screenPointMax),
            new Bounds2(viewportMin, viewportMax)
        );
    }
}