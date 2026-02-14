CREATE SEQUENCE seq_user_position
START 1
INCREMENT 1;

CREATE TABLE users (
    id_user VARCHAR(20) PRIMARY KEY,
    user_position INTEGER NOT NULL UNIQUE DEFAULT nextval('seq_user_position'),

    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    dni VARCHAR(20) NOT NULL UNIQUE,
    professional_license VARCHAR(50),
    specialty VARCHAR(100),
    university VARCHAR(150),
    professional_association_registration VARCHAR(150),
    phone_number VARCHAR(30),

    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,

    is_active BOOLEAN DEFAULT TRUE,

    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE user_logins (
    id_login SERIAL PRIMARY KEY,
    id_user VARCHAR(20) NOT NULL,
    email VARCHAR(150) NOT NULL,
    login_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ip_address VARCHAR(100),
    is_success BOOLEAN,

    CONSTRAINT fk_login_user
        FOREIGN KEY (id_user)
        REFERENCES users(id_user)
        ON DELETE CASCADE
);

CREATE TABLE password_reset_codes (
    id_code SERIAL PRIMARY KEY,
    id_user VARCHAR(20) NOT NULL,
    email VARCHAR(150) NOT NULL,
    recovery_code VARCHAR(10) NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    is_used BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_reset_user
        FOREIGN KEY (id_user)
        REFERENCES users(id_user)
        ON DELETE CASCADE
);

CREATE TABLE user_tokens (
    id_token SERIAL PRIMARY KEY,
    id_user VARCHAR(20) NOT NULL,
    refresh_token TEXT NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    is_revoked BOOLEAN DEFAULT FALSE,
    device_info TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_token_user
        FOREIGN KEY (id_user)
        REFERENCES users(id_user)
        ON DELETE CASCADE
);

CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_reset_email ON password_reset_codes(email);
CREATE INDEX idx_tokens_user ON user_tokens(id_user);

ALTER TABLE users
ADD CONSTRAINT email_lowercase CHECK (email = LOWER(email));
