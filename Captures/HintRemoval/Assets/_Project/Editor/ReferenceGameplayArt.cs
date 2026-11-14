
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
public static class ReferenceGameplayArt
{
    public const string Folder="Assets/_Project/UI/ReferenceArt";
    private static Texture2D source;
    private static Texture2D Crop(int x,int top,int w,int h)
    {
        var tex=new Texture2D(w,h,TextureFormat.RGBA32,false);
        var pixels=source.GetPixels(x,source.height-top-h,w,h);
        for(int i=0;i<pixels.Length;i++)if(pixels[i].a<.06f)pixels[i]=Color.clear;
        tex.SetPixels(pixels);tex.Apply();return tex;
    }
    private static void Clear(Texture2D tex,int x,int top,int w,int h)
    {
        for(int y=0;y<h;y++)for(int col=0;col<w;col++)
            if(x+col<tex.width&&top+y<tex.height)
                tex.SetPixel(x+col,tex.height-top-y-1,Color.clear);
        tex.Apply();
    }
    private static void Wood(Texture2D tex,int x,int top,int w,int h)
    {
        for(int y=0;y<h;y++)for(int col=0;col<w;col++){
            float dx=Mathf.Max(6-col,0,col-(w-7)),dy=Mathf.Max(6-y,0,y-(h-7));
            if(dx*dx+dy*dy>36)continue;
            float grain=Mathf.PerlinNoise(col*.07f,y*.3f)*.055f;
            tex.SetPixel(x+col,tex.height-top-y-1,new Color(.19f+grain,.14f+grain*.6f,.13f+grain*.5f,1));}
        tex.Apply();
    }
    private static void Save(Texture2D tex,string name,Vector4 border)
    {
        string path=Folder+"/"+name+".png";File.WriteAllBytes(path,tex.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(tex);AssetDatabase.ImportAsset(path);
        var importer=(TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Single;
        importer.spritePixelsPerUnit=100;importer.alphaIsTransparency=true;
        importer.mipmapEnabled=false;importer.filterMode=FilterMode.Bilinear;
        importer.textureCompression=TextureImporterCompression.Uncompressed;importer.spriteBorder=border;
        var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);
        settings.spriteMeshType=SpriteMeshType.FullRect;importer.SetTextureSettings(settings);importer.SaveAndReimport();
    }
    [MenuItem("Block Blast/UI/Extract Reference Gameplay Art")]
    public static void Generate()
    {
        Directory.CreateDirectory(Folder);
        source=new Texture2D(2,2);source.LoadImage(File.ReadAllBytes("Assets/_Project/sprite/Bộ giao diện game xếp khối .png"));
        try{
            Save(Crop(12,56,120,281),"BackgroundCliffs",Vector4.zero);
            var hud=Crop(17,376,432,97);
            Wood(hud,94,35,62,34);Wood(hud,180,35,117,34);
            Save(hud,"HUDWood",Vector4.zero);
            Save(Crop(329,395,47,57),"SettingsButton",Vector4.zero);
            Save(Crop(380,395,47,57),"HintButton",Vector4.zero);
            var board=Crop(429,653,292,260);
            Clear(board,32,27,234,191);
            Save(board,"BoardStone",new Vector4(36,46,36,36));
            Save(Crop(719,665,77,78),"CellBlue",new Vector4(18,18,18,18));
            var tray=Crop(826,697,286,186);
            Wood(tray,24,54,69,95);Wood(tray,104,54,77,95);Wood(tray,188,54,76,95);
            Save(tray,"TrayWood",Vector4.zero);
            var crown=Crop(631,50,46,46);Clear(crown,0,32,46,14);
            Save(crown,"Crown",Vector4.zero);
            var crest=Crop(935,697,65,48);Clear(crest,0,43,65,5);
            Clear(crest,0,31,10,12);Clear(crest,55,31,10,12);
            for(int y=0;y<crest.height;y++)for(int x=0;x<crest.width;x++)
                if(Mathf.Abs((x-24)/24f)+Mathf.Abs((y-16)/17f)>1.08f)
                    crest.SetPixel(x,crest.height-y-1,Color.clear);
            crest.Apply();
            Save(crest,"BoardCrest",Vector4.zero);
            int[] xs={1132,1210,1285,1361,1438};
            string[] names={"Red","Orange","Yellow","Green","Blue"};
            for(int i=0;i<xs.Length;i++)Save(Crop(xs[i],391,75,77),"Block"+names[i],Vector4.zero);
            Save(Crop(1132,481,75,78),"BlockCyan",Vector4.zero);
            Save(Crop(1210,481,75,78),"BlockPink",Vector4.zero);
            Save(Crop(1285,481,75,78),"BlockWhite",Vector4.zero);
        }finally{UnityEngine.Object.DestroyImmediate(source);source=null;}
        Variants();
        AssetDatabase.Refresh();
    }
    private static void Variants()
    {
        var purple=new Texture2D(2,2);
        purple.LoadImage(File.ReadAllBytes(Folder+"/BlockPink.png"));
        var pixels=purple.GetPixels();
        for(int i=0;i<pixels.Length;i++){
            float h,s,v;Color.RGBToHSV(pixels[i],out h,out s,out v);
            if(s>.15f){var c=Color.HSVToRGB(.76f,s,v);c.a=pixels[i].a;pixels[i]=c;}
        }
        purple.SetPixels(pixels);purple.Apply();Save(purple,"BlockPurple",Vector4.zero);
        foreach(string name in new[]{"Blue","Yellow","Red","Green","Cyan","Purple","Orange","Pink","White"})
        foreach(string state in new[]{"Highlighted","Disabled"}){
            var tex=new Texture2D(2,2);tex.LoadImage(File.ReadAllBytes(Folder+"/Block"+name+".png"));
            var colors=tex.GetPixels();
            for(int i=0;i<colors.Length;i++){float alpha=colors[i].a;var c=colors[i];
                c=state=="Highlighted"?Color.Lerp(c,Color.white,.18f):Color.Lerp(c,new Color(c.grayscale,c.grayscale,c.grayscale),.65f)*.65f;
                c.a=alpha;colors[i]=c;}
            tex.SetPixels(colors);tex.Apply();Save(tex,"Block"+name+state,Vector4.zero);
        }
    }
}
