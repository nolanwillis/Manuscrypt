import React, { useState, useRef } from 'react';

export default function CreatePost({ onChannelCreated }) {
    const nameRef = useRef('');
    const descriptionRef = useRef('');
    const [error, setError] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);
        try {
            const name = nameRef.current.value;
            if (!name) {
                throw new Error('File is required.');
            }
            const description = descriptionRef.current.value;
            let createdAt = new Date().toISOString();

            const response = await fetch('http://localhost:5125/Channel', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    name,
                    description,
                    createdAt
                }),
            });

            if (!response.ok) {
                const errorMsg = await response.text();
                throw new Error(errorMsg || 'Failed to create channel');
            }

            const createdChannel = await response.json();
            nameRef.current.value = '';
            descriptionRef.current.value = '';

            if (onChannelCreated) {
                onChannelCreated(createdChannel);
            }
        } catch (err) {
            setError(err.message);
        }
        finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="create-channel-form">
            <h2>Create New Channel</h2>
            {error && <div className="error">{error}</div>}
            <div>
                <label htmlFor="channel-name">Name</label>
                <input
                    id="channel-name"
                    type="text"
                    ref={nameRef}
                    disabled={isSubmitting}
                />
            </div>
            <div>
                <label htmlFor="channel-description">Description</label>
                <textarea
                    id="channel-description"
                    ref={descriptionRef}
                    disabled={isSubmitting}
                />
            </div>
            <button type="submit" disabled={isSubmitting}>
                {isSubmitting ? 'Creating...' : 'Create Channel'}
            </button>
        </form>
    );
}














