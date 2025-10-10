import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import CreatePost from './components/CreatePost.jsx'
import CreateChannel from './components/CreateChannel.jsx'

createRoot(document.getElementById('root')).render
(
    <StrictMode>
        <CreateChannel />
    </StrictMode>,
)
