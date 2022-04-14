#define Pi 3.141592653589793

unsigned long time = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  double val = 0.0;
  for(int deg = 0; deg < 360; deg++){
    double rad = deg / (180 / Pi);
    val = sin(rad);
    time = millis();
    Serial.print(time);
    Serial.print(" ");
    Serial.println(rad);
    delay(5);
    //delayMicroseconds(10);
  }
}
