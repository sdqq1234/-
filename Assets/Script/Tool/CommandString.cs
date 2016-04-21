using UnityEngine;
using System.Collections;

public static class CommandString {

    public static string BulletPrefabPath = "Prefabs/BulletPrefabs/";
    public static string ShooterPrefabPath = "Prefabs/ShooterPrefabs/";
    public static string CharactorPrefabPath = "Prefabs/CharactorPrefabs/";
    public static string EnemyPrefabPath = "Prefabs/EnemyPrefabs/";
    public static string SoundPath = "Sound/";
    public static string BGMPath = "BGM/";
    public static string EffectPath = "Prefabs/Effect/";
    public static string ItemPath = "Prefabs/Item/";

    public static string MyBulletLayer = "MyBullet";
    public static string EnemyBulletLayer = "EnemyBullet";
    public static string MyPlaneLayer = "MyPlane";
    public static string ShooterLayer = "Shooter";
    public static string EnemyPlaneLayer = "EnemyPlane";
    public static string ItemLayer = "Item";

    
    public static short FriendGroup = 1;//我方组id
    public static short EnemyGroup = 10;//敌方组id

    public static short MenuSceneID = 0;//菜单场景编号
    public static short GameStageSceneID = 1;//游戏场景编号

    public static Color EnemyRedColor = new Color(238f / 255f, 58f / 255f, 58f / 255f);
    public static Color EnemyBlueColor = new Color(47f / 255f, 91f / 255f, 255f / 255f);
    public static Color EnemyGreenColor = new Color(135f / 255f, 255f / 255f, 151f / 255f);
    public static Color EnemyYellowColor = new Color(246f / 255f, 255f / 255f, 159f/ 255f);
    public static Color EnemyPurpleColor = new Color(188f / 255f, 82f / 255f, 255f / 255f);//紫色
    public static Color EnemyPinkColor = new Color(255f / 255f, 142f / 255f, 201f / 255f);
    public static Color EnemyOrangeColor = new Color(255f / 255f, 176f / 255f, 66f / 255f);
    public static Color EnemyCyanColor = new Color(51f / 255f, 186f / 255f, 220f / 255f); //青色
    public static Color EnemyBulletColor = Color.gray;//灰色表示子弹颜色
    public static int ScreenRow = 5;//屏幕分5行
    public static int ScreenCol = 20;//屏幕分20列
}
