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

  if (loading) {
    return <p>Loading scoreboard...</p>;
  }

  return (
    <div className='center'>
      <div>
        <table>
          <thead>
            <tr>
              <th>Place</th>
              <th>Player</th>
              <th>Simon score</th>
              <th>Cardflip score</th>
              <th>NRG score</th>
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
                <td colSpan={3}>No scores available</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
