shader_type spatial;

uniform float teleport_range_x : hint_range(0.0, 1.0); // Диапазон телепортации по оси X
uniform float teleport_range_y : hint_range(0.0, 1.0); // Диапазон телепортации по оси Y
uniform float teleport_range_z : hint_range(0.0, 1.0); // Диапазон телепортации по оси Z
uniform float teleport_frequency : hint_range(0.0, 100.0); // Частота телепортации
uniform bool enable_blinking = true; // Включение/выключение мигания
uniform float blink_frequency : hint_range(0.0, 100.0); // Частота мигания

// Простая функция для создания случайных значений
float hash(float p) {
    p = fract(p * 0.3183099 + 0.1);
    p *= 17.0;
    return fract(p * (p + 31.0));
}

vec3 random_offset(float seed) {
    return vec3(
        hash(seed),
        hash(seed + 1.0),
        hash(seed + 2.0)
    ) * 2.0 - 1.0;
}

void vertex() {
    // Вычисляем текущий "кадр" телепортации на основе частоты
    float teleport_step = floor(TIME * teleport_frequency);

    // Генерация случайного смещения
    vec3 offset = random_offset(teleport_step);

    // Масштабируем смещение по каждой оси
    offset.x *= teleport_range_x;
    offset.y *= teleport_range_y;
    offset.z *= teleport_range_z;

    // Смещаем всю модель
    MODELVIEW_MATRIX[3].xyz += offset;
}

void fragment() {
    if (enable_blinking) {
        // Включаем мигание с частотой blink_frequency
        float blink_phase = fract(TIME * blink_frequency);
        if (blink_phase < 0.5) {
            discard; // Убираем пиксели, делая модель невидимой
        }
    }

    // Оставляем стандартный фрагментный вывод для видимой модели
}
