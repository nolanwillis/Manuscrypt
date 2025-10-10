import React, { useState, useRef } from 'react';

export default function CreatePost({ onPostCreated })
{
    const titleRef = useRef('');
    const [isForChildren, setIsForChildren] = useState(false);
    const [file, setFile] = useState(null);
    const [error, setError] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleFileChange = (e) => {
        e.preventDefault();
        setFile(e.target.files[0] || null);
    };

    let isForChildrenTimeout;
    const handleIsForChildrenChange = (e) => {
        e.preventDefault();
        clearTimeout(isForChildrenTimeout);
        const value = e.target.checked;
        isForChildrenTimeout = setTimeout(() => {
            setIsForChildren(value);
        }, 300); 
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);
        try {
            if (!file) {
                throw new Error('File is required.');
            }
            const title = titleRef.current.value;
            if (!title) {
                throw new Error('Title is required.');
            }
            let publishedAt = new Date().toISOString();
            let fileName = file.name;
            let fileType = file.type;
            let fileSizeBytes = file.size;

            const response = await fetch('https://localhost:5125/Post', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    channelId: 0, // Blank for now
                    title,
                    publishedAt,
                    isForChildren,
                    fileUrl: '', // Blank for now
                    fileName,
                    fileType,
                    fileSizeBytes,
                }),
            });

            if (!response.ok) {
                const errorMsg = await response.text();
                throw new Error(errorMsg || 'Failed to create post');
            }

            const createdPost = await response.json();
            setTitle('');
            setIsForChildren(false);
            setFile(null);

            if (onPostCreated) {
                onPostCreated(createdPost);
            }
        } catch (err) {
            setError(err.message);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="create-post-form">
            <h2>Create New Post</h2>
            {error && <div className="error">{error}</div>}
            <div>
                <label htmlFor="post-title">Title</label>
                <input
                    id="post-title"
                    type="text"
                    ref={titleRef}
                    disabled={isSubmitting}
                />
            </div>
            <div>
                <label>
                    <input
                        type="checkbox"
                        checked={isForChildren}
                        onChange={handleIsForChildrenChange}
                        disabled={isSubmitting}
                    />
                    Is For Children
                </label>
            </div>
            <div>
                <label htmlFor="post-file">File</label>
                <input
                    id="post-file"
                    type="file"
                    onChange={handleFileChange}
                    disabled={isSubmitting}
                />
            </div>
            <button type="submit" disabled={isSubmitting}>
                {isSubmitting ? 'Posting...' : 'Create Post'}
            </button>
        </form>
    );
}
