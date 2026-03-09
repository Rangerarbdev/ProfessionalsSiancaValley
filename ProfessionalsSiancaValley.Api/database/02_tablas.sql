CREATE TABLE users (
    id_user VARCHAR(20) PRIMARY KEY,
    user_position INTEGER GENERATED ALWAYS AS IDENTITY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    dni VARCHAR(20) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    estado_edad BOOLEAN NOT NULL,
    professional_license VARCHAR(50) NOT NULL,
    specialty VARCHAR(100) NOT NULL,
    university VARCHAR(150) NOT NULL,
    professional_association_registration VARCHAR(150) NOT NULL,
    phone_number VARCHAR(30) NOT NULL,
    email VARCHAR(150) NOT NULL,
    password_hash TEXT NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    role VARCHAR(30) NOT NULL DEFAULT 'User',
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);

CREATE TABLE miniatures (
    id SERIAL PRIMARY KEY,
    id_user VARCHAR(20) REFERENCES users(id_user),
    title VARCHAR(200),
    description TEXT,
    image_url TEXT,
    likes INT DEFAULT 0,
    dislikes INT DEFAULT 0,
    status VARCHAR(20) DEFAULT 'Pendiente',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE report (
    id SERIAL PRIMARY KEY,
    id_miniature INT REFERENCES miniatures(id),
    id_user VARCHAR(20) REFERENCES users(id_user),
    reason TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);