import { useState, useEffect } from 'react';
import './Simon.css';
import Keypad from '../assets/Keypad.png';
import Follow from '../assets/Follow.png';

interface Data {
  createdList: number[];
  level: number;
  expectedList: number[];
  difficulty: 'VeryEasy' | 'Easy' | 'Medium' | 'Hard' | 'Nightmare' | 'Impossible';
}

function Simon() {
  const [datas, setData] = useState<Data>();
  const [dataString1, setDataString1] = useState<string>(''); 
  const [dataString2, setDataString2] = useState<string>(''); 
  const [flashingButtons, setFlashingButtons] = useState(Array(9).fill(false));
  const [score, setScore] = useState<number>(0);
  const [hasFlashed, setHasFlashed] = useState<boolean>(false);

  const contents = datas === undefined
    ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
    : (
      <div>
        <div>{dataString1}</div>
        <div>{dataString2}</div>
        {score !== null && <h2>Your Score: {score}</h2>}
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

  const evaluateScore = async (userInput: number[]) => {
    if (!datas) return;

    try {
      const response = await fetch('https://localhost:5219/api/score/evaluate', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          userInput,
          pattern: datas.expectedList, 
          difficulty: datas.difficulty,
        }),
      });

      if (!response.ok) {
        throw new Error(`Error evaluating score: ${response.statusText}`);
      }

      const result = await response.json();
      setScore(result.score); 
    } catch (error) {
      console.error('Failed to evaluate score:', error);
    }
  };

  // Fetches data for the game
  const populateData = async () => {
    try {
      const response = await fetch('https://localhost:7005/api/Inc');
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      const results = await response.json();
      const data: Data = results.data;
      setData(data);
      const dataString1 = data.createdList.join(', ');
      setDataString1(dataString1);
    } catch (error) {
      console.error('Failed to fetch data:', error);
    }
  };

  const handleArray = () => {
    if (datas && datas.level >= 4) {
      for (let i = 0; i < datas.level; i++) {
        setTimeout(() => { handleFlash(datas.createdList[i] - 1); }, i * 400);
      }
    } else {
      console.error("Data is either undefined or doesn't have enough elements");
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
      const response = await fetch('https://localhost:5219/api/Inc', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
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

  return (
    <div className="center">
      <div>
        <div className="image-container">
          <img src={Follow} alt="Follow Image" className="image" />
          {buttonPositions.map((pos, index) => (
            <button
              key={index}
              className={`image-button ${flashingButtons[index] ? 'flashing' : ''}`}
              style={{ top: pos.top, left: pos.left, width: '50px', height: '50px' }}
            ></button>
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
                      evaluateScore(updatedUserInput);
                      setHasFlashed(false);
                    }
                  } else {
                    setScore(0);
                  }
                }
              }}
            ></button>
          ))}
        </div>

        <div>{contents}</div>
      </div>
    </div>
  );
}

export default Simon;
