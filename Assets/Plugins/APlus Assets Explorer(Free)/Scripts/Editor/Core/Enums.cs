//  Copyright (c) 2016-2017 amlovey
//  
#if UNITY_EDITOR
namespace APlusFree
{
    public enum APAssetType
    {
        Texture = 1,
        MovieTexture,
        Sprite,
        AnimationClip,
        AudioClip,
        AudioMixer,
        Font,
        GUISkin,
        Material,
        Mesh,
        Model,
        PhysicMaterial,
        PhysicsMaterial2D,
        Prefab,
        Scene,
        Script,
        Shader,
        StreamingAssets,
        Blacklist,
        Others,
        None,
    }

    public class AssetType 
    {
        public const string TEXTURES = "textures";
        public const string MOVIES = "movies";
    }
    
    public enum Platforms
    {
        Default,
        Standalone,
        iPhone,
        Web,
        WebGL,
        Android,
        Tizen
    }

    public enum MaterialType
	{
		Material,
		PhysicMaterial,
        PhysicsMaterial2D
	}
}
#endif