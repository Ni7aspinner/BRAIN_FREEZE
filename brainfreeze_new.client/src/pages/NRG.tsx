import { useState, useEffect} from 'react';
import './NRG.css';
import Grid from '../assets/Grid-1000-10-2-100.png';
interface Data {
  createdList: number[];
  level: number;
  expectedList: number[]; 
  difficulty: 'VeryEasy' | 'Easy' | 'Medium' | 'Hard' | 'Nightmare' | 'Impossible';
}

function NRG() {
  const [datas, setData] = useState<Data>();
  const [dataString1, setDataString1] = useState<string>(''); 
  const [dataString2, setDataString2] = useState<string>(''); 

    const [flashingButtons, setFlashingButtons] = useState(Array(25).fill(false));
    const buttonPositions = [
        { top: '0.2%', left: '0%' },
        { top: '0.2%', left: '20%' },
        { top: '0.2%', left: '40%' },
        { top: '0.2%', left: '60%' },
        { top: '0.2%', left: '80%' },

        { top: '20%', left: '0%' },
        { top: '20%', left: '20%' },
        { top: '20%', left: '40%' },
        { top: '20%', left: '60%' },
        { top: '20%', left: '80%' },

        { top: '39.8%', left: '0%' },
        { top: '39.8%', left: '20%' },
        { top: '39.8%', left: '40%' },
        { top: '39.8%', left: '60%' },
        { top: '39.8%', left: '80%' },
        
        { top: '59.6%', left: '0%' },
        { top: '59.6%', left: '20%' },
        { top: '59.6%', left: '40%' },
        { top: '59.6%', left: '60%' },
        { top: '59.6%', left: '80%' },
        
        { top: '79.3%', left: '0%' },
        { top: '79.3%', left: '20%' },
        { top: '79.3%', left: '40%' },
        { top: '79.3%', left: '60%' },
        { top: '79.3%', left: '80%' },
  ];

  useEffect(() => {
    populateData();
  }, []);
  useEffect(() => {
    handleArray();
  }, [datas?.createdList]);
  const populateData = async () => {
    try {
        console.log('Populating data');
        const response = await fetch(`https://localhost:7005/api/NRG`); 
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

  async function postData(data : Data) 
  {
    try {
      console.log('Posting data:', JSON.stringify(data));
      const response = await fetch('https://localhost:5219/api/NRG', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
      });

      if (response.ok) {
      const results = await response.json();
      const result = results.data;
      console.log(results.message);
      console.log('API response: ', result);
      setData(result);
      const dataString1 = result.createdList.join(', '); 
      setDataString1(dataString1); 
      const dataString2 = result.expectedList.join(', ');
      setDataString2(dataString2);
      }
      else {
        console.error('Error in API request: ', response.statusText);
      }
    } 
    catch (error) {
      console.error("Failed to fetch data", error);
    }
  } 
  
  const handleArray = () => {
    if (datas && datas.level >=4) {
      for(let i=0; i<datas.level; i++){
        flashButton(datas.createdList[i]);
      }
      setTimeout(() => {setFlashingButtons(Array(25).fill(false));},2000);
    } 
    else{
      console.error("Data is either undefined or doesn't have enough elements");
    }
  }; 
  const flashButton = (index: number) => {
    setFlashingButtons((prev) => {
      const newState = [...prev];
      newState[index] = true;
      return newState;
    });
  };
    return (
      <div className='center'>
        <><div className="block" >
              <div className='image-container1'>
              <img src={Grid} className="imageGrid"/>
              {buttonPositions.map((pos, index) => (
            <button
                  key={index}
                  className={`grid-block ${flashingButtons[index] ? 'activated' : ''}`}
                  style={{ top: pos.top, left: pos.left, width: '100px', height: '100px' }}
                  onClick={() => {
                      flashButton(index);
                      if (datas) {
                        const updatedData = {
                            ...datas,
                            expectedList: Array.isArray(datas.expectedList) ? [...datas.expectedList, index] : [index],
                        };
                        setData(updatedData);
                        const dataString2 = updatedData.expectedList.join(', ');
                        setDataString2(dataString2);
                        if(updatedData.createdList.length==updatedData.expectedList.length){
                          setFlashingButtons(Array(25).fill(false));
                          postData(updatedData);
              
                        }
                      }
                  }}
            >
            {datas && datas.createdList.includes(index)? datas.createdList.indexOf(index) + 1 : ''}
            </button>
          ))}
          <div>{dataString1}</div>
          <div>{dataString2}</div>
          <button onClick={handleArray}></button>
          <div >(index starts from zero)</div>
          </div>
          </div>
        </>
      </div>
    );
  }
export default NRG;