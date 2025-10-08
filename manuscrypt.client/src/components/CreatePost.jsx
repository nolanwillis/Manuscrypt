import React, { useState } from 'react';

export default function CreatePost({onPostCreated})
{
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [error, setError] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    if (!title.trim() || !content.trim()) {
      setError('Title and content are required.');
      return;
    }

    setIsSubmitting(true);
    try {
      // Placeholder: log the data instead of submitting
      console.log('Submitting post:', { title, content });
      setTitle('');
      setContent('');
      if (onPostCreated) onPostCreated({ title, content });
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
          value={title}
          onChange={e => setTitle(e.target.value)}
          disabled={isSubmitting}
        />
      </div>
      <div>
        <label htmlFor="post-content">Content</label>
        <textarea
          id="post-content"
          value={content}
          onChange={e => setContent(e.target.value)}
          disabled={isSubmitting}
        />
      </div>
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Posting...' : 'Create Post'}
      </button>
    </form>
  );
};
