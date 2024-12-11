import { Link } from 'react-router-dom';
import './Navigationbar.css';
import { useState, useEffect } from 'react';

function Header() {
    const [isMuted, setIsMuted] = useState(false);

    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    useEffect(() => {
        fetch(`${backendUrl}Mute`) 
            .then(response => response.json())
            .then(data => {
                setIsMuted(data.isMuted);  
            })
            .catch(error => {
                console.error('Error fetching mute state:', error);
            });
    }, []);

    const handleMuteToggle = () => {
        const newMuteState = !isMuted;

        fetch(`${backendUrl}Mute`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ isMuted: newMuteState }),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Mute state updated:', data);
        })
        .catch(error => {
            console.error('Error updating mute state:', error);
        });

        setIsMuted(newMuteState);
    };

    return (
        <header>
            <nav>
                <Link to="/home" className="home-arrow"></Link>
                <button className="mute-button" onClick={handleMuteToggle}>
                    {isMuted ? 'Unmute' : 'Mute'}
                </button>
            </nav>
        </header>
    );
}

export default Header;
