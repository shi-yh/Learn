using System.Collections.Generic;

public class DefenceEnemyCollection
{
    private List<DefenceEnemy> _enemies = new List<DefenceEnemy>();

    public void Add(DefenceEnemy enemy)
    {
        _enemies.Add(enemy);
    }


    public void GameUpdate()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (!_enemies[i].GameUpdate())
            {
                int lastIndex = _enemies.Count - 1;
                _enemies[i] = _enemies[lastIndex];
                _enemies.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }
}