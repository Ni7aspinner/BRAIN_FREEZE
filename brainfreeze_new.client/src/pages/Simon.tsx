import { useState, useEffect } from 'react';
import './Simon.css';
import Keypad from '../assets/Keypad.png';
import Follow from '../assets/Follow.png';

interface Data {
    createdList: number[];
    level: number;
    expectedList: number[];
}

enum Difficulty {
  MainStart = 1,
  VeryEasy = 4,
  Easy = 5,
  Medium = 6,
  Hard = 7,
  Nightmare = 8,
  Impossible = 9,
  Custom = 0,
}

enum GameMode {
  Main = 'Main',
  Practice = 'Practice'
}

function Simon() {
  const [datas, setData] = useState<Data>();
  const [dataString1, setDataString1] = useState<string>('');
  const [dataString2, setDataString2] = useState<string>('');
  const [flashingButtons, setFlashingButtons] = useState(Array(9).fill(false));
  const [score, setScore] = useState<number>(0);
  const [hasFlashed, setHasFlashed] = useState<boolean>(false);
  const [gameMode, setGameMode] = useState<GameMode>(GameMode.Main); 
  const [currentDifficulty, setCurrentDifficulty] = useState<Difficulty>(Difficulty.MainStart);
  const [id] = useState<string | null>(localStorage.getItem("ID"));

  const contents = datas === undefined
      ? <p><em>Loading... </em></p>
      : (
          <div>
            <div>{dataString1}</div>
            <div>{dataString2}</div>
            <h2>{gameMode}</h2>
            <p>Difficulty selected: {Difficulty[currentDifficulty]}</p>
            {score !== null && <h2>Your Score: {score}</h2>} {/* Display score */}
          </div>
      );

  const buttonPositions = [
    { top: '40%', left: '28.5%' },
    { top: '40%', left: '51%' },
    { top: '40%', left: '71%' },
    { top: '58%', left: '28.5%' },
    { top: '58%', left: '51%' },
    { top: '58%', left: '71%' },
    { top: '77.5%', left: '28.5%' },
    { top: '77.5%', left: '51%' },
    { top: '77.5%', left: '71%' },
  ];

  useEffect(() => {
    fetchSimonScore();
    populateData();
  }, []);

  useEffect(() => {
    if (datas && !hasFlashed) {
      setTimeout(() => {
        handleArray();
        setHasFlashed(true);
      }, 1000);
    }
  }, [datas, hasFlashed]);

  useEffect(() => {
    // Whenever the game mode or difficulty changes, trigger new data population
    if (datas) {
      console.log('Game mode or difficulty changed, populating new data...');
      setHasFlashed(false); // Reset so that it flashes again
      setData(undefined); // Clear current data
      populateData(); // Populate data again based on current session
        // After this data is populated, so the other effect is triggered and flashing starts again
    }
  }, [gameMode, currentDifficulty]);

  const fetchSimonScore = async () => {
    try {
        const response = await fetch(`https://localhost:7005/api/Scoreboards/get-by-id/${id}`);
        if (!response.ok) {
            throw new Error(`Error fetching scores: ${response.statusText}`);
        }

        const user = await response.json();

        if (user) {
            localStorage.setItem("Simon", user.simonScore)
            console.log(user.simonScore)
        } else {
            console.warn(`User with ID ${user.id} not found.`);
        }
    } catch (error) {
        console.error("Error fetching user scores:", error);
    }
};

const putScore = async (newSimonScore: number) => {
  try {
      console.log(id);
      const fetchResponse = await fetch(`https://localhost:7005/api/Scoreboards/get-by-id/${id}`);
      if (!fetchResponse.ok) {
          throw new Error(`Error fetching user: ${fetchResponse.statusText}`);
      }

      const user = await fetchResponse.json();

      if (user) {
          const updatedUser = { ...user, simonScore: newSimonScore };

          const putResponse = await fetch(`https://localhost:7005/api/Scoreboards/${id}`, {
              method: "PUT",
              headers: {
                  "Content-Type": "application/json",
              },
              body: JSON.stringify(updatedUser),
          });

          if (!putResponse.ok) {
              throw new Error(`Error updating user: ${putResponse.statusText}`);
          }

          console.log(`User with ID ${id} updated successfully.`);
          localStorage.setItem("Simon", updatedUser.simonScore.toString());
      } else {
          console.warn(`User with ID ${id} not found.`);
      }
  } catch (error) {
      console.error("Error updating user score:", error);
  }
};



    const evaluateScore = async (userInput: number[], datas: Data) => {

        if (!datas) return;
        console.log("Expected Pattern:", datas.expectedList);
        console.log("User Input:", userInput);
    try {
      const response = await fetch('https://localhost:5219/api/score/evaluate', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          userInput,
          pattern: datas.expectedList,
          difficulty: currentDifficulty,
        }),
      });

      if (!response.ok) {
        throw new Error(`Error evaluating score: ${response.statusText}`);
      }

      const result = await response.json();
      setScore(result.score); 
      const previousHighScore = Number(localStorage.getItem('Simon')) || 0;
      if (result.score > previousHighScore) {
        localStorage.setItem('Simon', result.score.toString());
        putScore(result.score);
      }
    } catch (error) {
      console.error('Failed to evaluate score:', error);
    }
  };
  
  // Fetches data for the game

  const populateData = async () => {
    try {
        console.log('Populating data');
        const response = await fetch(`https://localhost:7005/api/Data?level=${currentDifficulty}`, {
            method: 'GET'
        });
        if (!response.ok) {       
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const results = await response.json();
        const data: Data = results.data;
        console.log(results.message);
        setData(data);

        const dataString1 = data.createdList.join(', ');
        setDataString1(dataString1);
    } catch (error) {
      console.error('Failed to fetch data:', error);
    }
  };

  const handleArray = () => {
    if (datas && datas.level >=1) {
      for(let i=0; i<datas.level; i++){
        setTimeout(() => { handleFlash(datas.createdList[i]-1);}, i*400);
      }
    } 
    else{
      console.error("Data is either undefined or doesn't have enough elements ");
    }
  };

  const handleFlash = (index: number) => {
    setFlashingButtons((prev) => {
      const newState = [...prev];
      newState[index] = true;
      return newState;
    });

    setTimeout(() => {
      setFlashingButtons((prev) => {
        const newState = [...prev];
        newState[index] = false;
        return newState;
      });
    }, 200);
  };

  async function postData(data: Data) {
    try {
      
        console.log('Posting data:', JSON.stringify(data));

        const payload = {
            ...data,
            difficulty: currentDifficulty
        };
      
      const response = await fetch('https://localhost:5219/api/Data', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
          body: JSON.stringify(payload),
      });

      if (response.ok) {
        const results = await response.json();
        setData(results.data);
        const dataString1 = results.data.createdList.join(', ');
        setDataString1(dataString1);
        const dataString2 = results.data.expectedList.join(', ');
        setDataString2(dataString2);
      } else {
        console.error('Error in API request:', response.statusText);
      }
    } catch (error) {
      console.error("Failed to fetch data", error);
    }
  }

  const togglePracticeMode = () => {
    setGameMode(GameMode.Practice);
    setCurrentDifficulty(Difficulty.VeryEasy);
    setScore(0); 
    console.log('Switching to practice mode');
  };

  const toggleMainMode = () => {
    setGameMode(GameMode.Main);
    setCurrentDifficulty(Difficulty.MainStart);
    setScore(0);
    console.log('Switching to main mode');
  }

  const handleDifficultyChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedDifficulty = Number(event.target.value) as Difficulty;
    setCurrentDifficulty(selectedDifficulty);
  };

    return (
        <>
            <div className="controls" style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                <button
                    onClick={gameMode === GameMode.Practice ? toggleMainMode : togglePracticeMode}
                    style={{ width: '250px', height: '60px' }}
                >
                    {gameMode === GameMode.Practice ? 'Switch to main mode' : 'Switch to practice mode'}
                </button>
            </div>

            <div className="controls" style={{ display: 'flex', flexDirection: 'column', justifyContent: 'center', alignItems: 'center' }}>
                {gameMode === GameMode.Practice && (
                    <select
                        value={currentDifficulty}
                        onChange={handleDifficultyChange}
                        style={{ marginBottom: '10px' }}
             >

            <option value="4">Very Easy</option>
            <option value="5">Easy</option>
            <option value="6">Medium</option>
            <option value="7">Hard</option>
            <option value="8">Nightmare</option>
            <option value="9">Impossible</option>
            <option value="0">Custom</option>
          </select>
        )}
      </div>
    <div className='center'>
      <div>
        <div className="image-container">
          <img src={Follow} alt="Follow Image" className="image" />
          {buttonPositions.map((pos, index) => (
            <button
              key={index}
              className={`image-button ${flashingButtons[index] ? 'flashing' : ''}`}
              style={{ top: pos.top, left: pos.left, width: '50px', height: '50px' }}
            />
          ))}
        </div>
        <div className="image-container">
          <img src={Keypad} alt="Keypad Image" className="image" />
          {buttonPositions.map((pos, index) => (
            <button
              key={index}
              className="image-button"
              style={{ top: pos.top, left: pos.left, width: '50px', height: '50px' }}
              onClick={() => {
                handleFlash(index);
                if (datas) {
                  const updatedUserInput = [...datas.expectedList, index + 1];
                  const updatedData = {
                    ...datas,
                    expectedList: Array.isArray(datas.expectedList) ? [...datas.expectedList, index + 1] : [index + 1],
                  };
                  setData(updatedData);
                  const dataString2 = updatedData.expectedList.join(', ');
                  setDataString2(dataString2);
                  postData(updatedData);
                  if (updatedUserInput.length === datas.createdList.length) {
                    if (JSON.stringify(updatedUserInput) === JSON.stringify(datas.createdList)) {
                      evaluateScore(updatedUserInput, updatedData);
                      setHasFlashed(false);
                    }
                  } else {
                    setScore(0);
                  }
                }
              }}
            />
          ))}
        </div>

        <div>{contents}</div>
      </div>
    </div>
  </>
);

}

export default Simon;
