import { useEffect, useState } from 'react';

type Score = {
  id: number;
  place: number;
  username: string;
  simonScore: number;
  cardflipScore: number;
  nrgScore: number;
};

export default function Scoreboard() {
  const [scores, setScores] = useState<Score[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [sortConfig, setSortConfig] = useState<{ key: keyof Score; direction: 'asc' | 'desc' } | null>(null);

  useEffect(() => {
    fetch('https://localhost:7005/api/Scoreboards')
      .then((response) => response.json())
      .then((data) => {
        setScores(data);
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error fetching scoreboard data:', error);
        setLoading(false);
      });
  }, []);

  const handleSort = (key: keyof Score) => {
    let direction: 'asc' | 'desc' = 'asc';
    if (sortConfig && sortConfig.key === key && sortConfig.direction === 'asc') {
      direction = 'desc';
    }
    setSortConfig({ key, direction });

    const sortedScores = [...scores].sort((a, b) => {
      if (a[key] < b[key]) {
        return direction === 'asc' ? -1 : 1;
      }
      if (a[key] > b[key]) {
        return direction === 'asc' ? 1 : -1;
      }
      return 0;
    });

    setScores(sortedScores);
  };

  if (loading) {
    return <p>Loading scoreboard...</p>;
  }

  return (
    <div className='center'>
      <div>
        <table>
          <thead>
            <tr>
              <th onClick={() => handleSort('place')}>Place {sortConfig?.key === 'place' ? (sortConfig.direction === 'asc' ? '↑' : '↓') : ''}</th>
              <th onClick={() => handleSort('username')}>Player {sortConfig?.key === 'username' ? (sortConfig.direction === 'asc' ? '↑' : '↓') : ''}</th>
              <th onClick={() => handleSort('simonScore')}>Simon Score {sortConfig?.key === 'simonScore' ? (sortConfig.direction === 'asc' ? '↑' : '↓') : ''}</th>
              <th onClick={() => handleSort('cardflipScore')}>Cardflip Score {sortConfig?.key === 'cardflipScore' ? (sortConfig.direction === 'asc' ? '↑' : '↓') : ''}</th>
              <th onClick={() => handleSort('nrgScore')}>NRG Score {sortConfig?.key === 'nrgScore' ? (sortConfig.direction === 'asc' ? '↑' : '↓') : ''}</th>
            </tr>
          </thead>
          <tbody>
            {scores.length > 0 ? (
              scores.map((score) => (
                <tr key={score.id}>
                  <td>{score.place}</td>
                  <td>{score.username}</td>
                  <td>{score.simonScore}</td>
                  <td>{score.cardflipScore}</td>
                  <td>{score.nrgScore}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={5}>No scores available</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
