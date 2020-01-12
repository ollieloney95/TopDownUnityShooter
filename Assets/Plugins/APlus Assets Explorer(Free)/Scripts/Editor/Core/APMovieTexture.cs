//  Copyright (c) 2016-2017 amlovey
//  
#if UNITY_EDITOR
namespace APlusFree
{
    [JSONRootAttribute]
    [System.SerializableAttribute]
    public class APMovieTexture : APAsset 
    {
        [JSONDataMemberAttribute]
        public string Duration { get; set; }
        
        [JSONDataMemberAttribute]
        public long Size { get; set; }
        
        [JSONDataMemberAttribute]
        public double Approx { get; set; }
        
        [JSONDataMemberAttribute]
        public float Quality { get; set; }
        
        public APMovieTexture()
        {
            APType = AssetType.MOVIES;
        }
    }
}
#endif