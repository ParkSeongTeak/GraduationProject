/*
�ۼ��� : �̿쿭
�ۼ��� : 23.04.05
�ֱ� ���� ���� : 23.04.05
�ֱ� ���� ���� : ��� �� �ε� ����� �� �Ŵ����� ���� ����
*/

namespace Client
{
    public class SceneManager
    {
        /// <summary> Enum���� ������ �� ��ȯ </summary>
        public static void LoadScene(Define.Scenes scene)
        {
            //ui popup �ʱ�ȭ
            GameManager.UI.Clear();
            //���� ������� �ʱ�ȭ
            GameManager.InGameData.Clear();
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
        }
    }
}
