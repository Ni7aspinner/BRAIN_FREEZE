import { Link, useNavigate } from 'react-router-dom';
import './Home.css';

export default function Home() {
    const navigate = useNavigate();
    const username = localStorage.getItem("token");
    const handleClearTokens = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('sessionId');
        console.log('Token cleared!');
        navigate('/');
    };

    return (
        <div className="button-container">
            <p>
                Welcome To BRAINFREEZE, {username ? username : "Guest"}!<br />
                Select a game to play!<br />And remember...
            </p>
            <div className="button-grid">
                <Link to="/card-flip"><button className="game-button">Card Flip</button></Link>
                <Link to="/simon"><button className="game-button">Simon</button></Link>
                <Link to="/nrg"><button className="game-button">NRG</button></Link>
                <Link to="/scoreboard"><button className="game-button">Scoreboard</button></Link>
            </div>

            <div className="settings-container">
                <button className="clear-button" onClick={handleClearTokens}>
                    <i className="fa fa-times-circle" aria-hidden="true"></i>
                    Clear Tokens
                </button>
            </div>
            <p>
                SessionID {localStorage.getItem("sessionId")}
            </p>
        </div>
    );
}
