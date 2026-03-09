CREATE INDEX idx_users_email
ON users(email);

CREATE INDEX idx_users_dni
ON users(dni);

CREATE INDEX idx_users_role
ON users(role);

CREATE INDEX idx_users_user_position
ON users(user_position);

CREATE INDEX idx_miniatures_user
ON miniatures(id_user);

CREATE INDEX idx_miniatures_created_at
ON miniatures(created_at);

CREATE INDEX idx_report_user
ON report(id_user);

CREATE INDEX idx_report_created_at
ON report(created_at);